namespace TaskTracker.Models;

public class Label
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Color { get; set; }
    public List<TaskItem> TaskItems { get; set; } = [];
}
