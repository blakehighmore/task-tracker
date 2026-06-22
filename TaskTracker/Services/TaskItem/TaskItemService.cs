using Microsoft.EntityFrameworkCore;
using TaskTracker.Data;
using TaskTracker.DTOs;
using TaskTracker.DTOs.Label;
using TaskTracker.DTOs.TaskItem;
using TaskTracker.Exceptions;


namespace TaskTracker.Services.TaskItem;

public class TaskItemService : ITaskItemService
{
    private readonly AppDbContext _db;

    public TaskItemService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<PagedResult<TaskItemReadDto>> GetAllAsync(TaskItemQueryParameters query)
    {
        int page = Math.Max(1, query.Page);
        int pageSize = Math.Clamp(query.PageSize, 1, 100);

        var itemTaskQuery = _db.TaskItems.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
            itemTaskQuery = itemTaskQuery.Where(t =>
                EF.Functions.ILike(t.Title, $"%{query.Search}%") || t.Description != null &&
                EF.Functions.ILike(t.Description, $"%{query.Search}%"));

        itemTaskQuery = query.SortBy?.ToLower() switch
        {
            "title" => query.Desc ? itemTaskQuery.OrderByDescending(t => t.Title) : itemTaskQuery.OrderBy(t => t.Title),
            "description" => query.Desc
                ? itemTaskQuery.OrderByDescending(t => t.Description)
                : itemTaskQuery.OrderBy(t => t.Description),
            _ => itemTaskQuery.OrderBy(t => t.Id)
        };
        int totalCount = await itemTaskQuery.CountAsync();

        var items = await itemTaskQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t =>
                new TaskItemReadDto(t.Id, t.Title, t.Description, t.Status, t.DueDate, t.ProjectId,
                    t.Labels.Select(l => new LabelReadDto(l.Id, l.Name, l.Color))
                        .ToList()))
            .ToListAsync();

        return new PagedResult<TaskItemReadDto>(items, page, pageSize, totalCount);
    }

    public async Task<TaskItemReadDto?> GetByIdAsync(int id)
    {
        var taskItem = await _db.TaskItems
            .AsNoTracking()
            .Where(t => t.Id == id)
            .Select(t =>
                new TaskItemReadDto(t.Id, t.Title, t.Description, t.Status, t.DueDate, t.ProjectId,
                    t.Labels.Select(l => new LabelReadDto(l.Id, l.Name, l.Color))
                        .ToList()))
            .FirstOrDefaultAsync();

        if (taskItem is null) throw new NotFoundException("Задача не была найдена");

        return taskItem;
    }

    public async Task<TaskItemReadDto> CreateAsync(TaskItemCreateDto dto)
    {
        var taskItem = new Models.TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            DueDate = dto.DueDate,
            ProjectId = dto.ProjectId,
            Status = dto.Status
        };
        _db.TaskItems.Add(taskItem);
        await _db.SaveChangesAsync();

        return new TaskItemReadDto(taskItem.Id, taskItem.Title, taskItem.Description, taskItem.Status, taskItem.DueDate,
            taskItem.ProjectId, []);
    }

    public async Task<bool> UpdateAsync(int id, TaskItemUpdateDto dto)
    {
        var taskItem = await _db.TaskItems.FindAsync(id);

        if (taskItem is null) return false;

        taskItem.Title = dto.Title;
        taskItem.Description = dto.Description;
        taskItem.DueDate = dto.DueDate;
        taskItem.Status = dto.Status;
        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var taskItem = await _db.TaskItems.FindAsync(id);

        if (taskItem is null) return false;

        _db.TaskItems.Remove(taskItem);
        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<bool> AddLabelAsync(int taskId, int labelId)
    {
        var task = await _db.TaskItems.Include(t => t.Labels)
            .FirstOrDefaultAsync(t => t.Id == taskId);

        if (task is null) return false;

        var label = await _db.Labels.FindAsync(labelId);

        if (label is null) return false;

        if (task.Labels.Any(l => l.Id == labelId)) return true;

        task.Labels.Add(label);
        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RemoveLabelAsync(int taskId, int labelId)
    {
        var task = await _db.TaskItems
            .Include(t => t.Labels)
            .FirstOrDefaultAsync(t => t.Id == taskId);

        if (task is null) return false;

        var label = task.Labels.FirstOrDefault(l => l.Id == labelId);

        if (label is null) return false;

        task.Labels.Remove(label);
        await _db.SaveChangesAsync();

        return true;
    }
}