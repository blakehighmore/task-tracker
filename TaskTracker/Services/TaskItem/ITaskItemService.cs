using TaskTracker.DTOs;
using TaskTracker.DTOs.TaskItem;


namespace TaskTracker.Services.TaskItem;

public interface ITaskItemService
{
    public Task<PagedResult<TaskItemReadDto>> GetAllAsync(TaskItemQueryParameters query);
    public Task<TaskItemReadDto?> GetByIdAsync(int id);
    public Task<TaskItemReadDto> CreateAsync(TaskItemCreateDto dto);
    public Task<bool> UpdateAsync(int id, TaskItemUpdateDto dto);
    public Task<bool> DeleteAsync(int id);
}