using TaskTracker.Models;


namespace TaskTracker.DTOs.TaskItem;

public record TaskItemUpdateDto(
    string Title,
    string? Description,
    TaskItemStatus Status,
    DateTime? DueDate);