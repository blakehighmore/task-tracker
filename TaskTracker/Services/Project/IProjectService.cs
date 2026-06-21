using TaskTracker.DTOs;
using TaskTracker.DTOs.Project;


namespace TaskTracker.Services.Project;

public interface IProjectService
{
    public Task<PagedResult<ProjectReadDto>> GetAllAsync(ProjectQueryParameters query);
    public Task<ProjectReadDto?> GetByIdAsync(int id);
    public Task<ProjectReadDto> CreateAsync(ProjectCreateDto dto);
    public Task<bool> UpdateAsync(int id, ProjectUpdateDto dto);
    public Task<bool> DeleteAsync(int id);
}