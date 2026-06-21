

namespace TaskTracker.DTOs.Project;

public record ProjectReadDto(int Id, string Name, string? Description, int OwnerId);