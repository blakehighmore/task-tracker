using TaskTracker.DTOs.Label;


namespace TaskTracker.Services.Label;

public interface ILabelService
{
    public Task<IEnumerable<LabelReadDto>> GetAllAsync();
    public Task<LabelReadDto?> GetByIdAsync(int id);
    public Task<LabelReadDto> CreateAsync(LabelCreateDto dto);
    public Task<bool> DeleteAsync(int id);
}