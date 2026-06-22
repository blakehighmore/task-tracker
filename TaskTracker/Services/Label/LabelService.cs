using Microsoft.EntityFrameworkCore;
using TaskTracker.Data;
using TaskTracker.DTOs.Label;


namespace TaskTracker.Services.Label;

public class LabelService : ILabelService
{
    private readonly AppDbContext _db;

    public LabelService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<LabelReadDto>> GetAllAsync()
    {
        return await _db.Labels.AsNoTracking().Select(l => new LabelReadDto(l.Id, l.Name, l.Color)).ToListAsync();
    }

    public async Task<LabelReadDto?> GetByIdAsync(int id)
    {
        return await _db.Labels
            .AsNoTracking()
            .Where(l => l.Id == id)
            .Select(l => new LabelReadDto(l.Id, l.Name, l.Color))
            .FirstOrDefaultAsync();
    }

    public async Task<LabelReadDto> CreateAsync(LabelCreateDto dto)
    {
        var label = new Models.Label { Name = dto.Name, Color = dto.Color };
        _db.Labels.Add(label);
        await _db.SaveChangesAsync();

        return new LabelReadDto(label.Id, label.Name, label.Color);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var label = await _db.Labels.FindAsync(id);

        if (label is null) return false;
        _db.Labels.Remove(label);
        await _db.SaveChangesAsync();

        return true;
    }
}