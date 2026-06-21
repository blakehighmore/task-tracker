using Microsoft.EntityFrameworkCore;
using TaskTracker.Models;


namespace TaskTracker.Data;

public class AppDbContext : DbContext
{
    
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }


    public DbSet<User> Users => Set<User>();
    public DbSet<Label> Labels => Set<Label>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<TaskItem> TaskItems => Set<TaskItem>();
}