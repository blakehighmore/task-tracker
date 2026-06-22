using TaskTracker.DTOs.Label;
using TaskTracker.Models;


namespace TaskTracker.DTOs.TaskItem;

public record TaskItemReadDto(
    int Id,
    string Title,
    string? Description,
    TaskItemStatus Status,
    DateTime? DueDate,
    int ProjectId,
    List<LabelReadDto> Labels);