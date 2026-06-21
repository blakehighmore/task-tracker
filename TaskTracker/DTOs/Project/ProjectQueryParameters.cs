namespace TaskTracker.DTOs.Project;

public class ProjectQueryParameters
{
    public string? Search { get; set; }
    public string? SortBy { get; set; }
    public bool Desc { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
};