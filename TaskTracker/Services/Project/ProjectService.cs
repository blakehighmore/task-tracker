using Microsoft.EntityFrameworkCore;
using TaskTracker.Data;
using TaskTracker.DTOs;
using TaskTracker.DTOs.Project;
using TaskTracker.Exceptions;


namespace TaskTracker.Services.Project;

public class ProjectService : IProjectService
{
    private readonly AppDbContext _db;

    public ProjectService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<PagedResult<ProjectReadDto>> GetAllAsync(ProjectQueryParameters query)
    {
        int page = Math.Max(1, query.Page);
        int pageSize = Math.Clamp(query.PageSize, 1, 100);
        var projectQuery = _db.Projects.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            projectQuery = _db.Projects.Where(p =>
                EF.Functions.ILike(p.Name, $"%{query.Search}%") || (p.Description != null) &&
                EF.Functions.ILike(p.Description, $"%{query.Search}%"));
        }

        projectQuery = query.SortBy?.ToLower() switch
        {
            "name" => query.Desc ? projectQuery.OrderByDescending(p => p.Name) : projectQuery.OrderBy(p => p.Name),
            "description" => query.Desc
                ? projectQuery.OrderByDescending(p => p.Description)
                : projectQuery.OrderBy(p => p.Description),
            _ => projectQuery.OrderBy(p => p.Id)
        };

        var totalCount = await projectQuery.CountAsync();


        var items = await projectQuery.Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProjectReadDto(p.Id, p.Name, p.Description, p.OwnerId))
            .ToListAsync();

        return new PagedResult<ProjectReadDto>(items, page, pageSize, totalCount);
    }

    public async Task<ProjectReadDto?> GetByIdAsync(int id)
    {
        var project = await _db.Projects
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new ProjectReadDto(p.Id, p.Name, p.Description, p.OwnerId))
            .FirstOrDefaultAsync();

        if (project is null) throw new NotFoundException("Проект не был найден");

        return project;
    }

    public async Task<ProjectReadDto> CreateAsync(ProjectCreateDto dto)
    {
        Models.Project project = new()
        {
            Name = dto.Name,
            Description = dto.Description,
            OwnerId = dto.OwnerId
        };
        _db.Projects.Add(project);
        await _db.SaveChangesAsync();

        return new ProjectReadDto(project.Id, project.Name, project.Description, project.OwnerId);
    }

    public async Task<bool> UpdateAsync(int id, ProjectUpdateDto dto)
    {
        var project = await _db.Projects.FindAsync(id);

        if (project is null) return false;

        project.Name = dto.Name;
        project.Description = dto.Description;

        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var project = await _db.Projects.FindAsync(id);

        if (project is null) return false;

        _db.Projects.Remove(project);
        await _db.SaveChangesAsync();

        return true;
    }
}