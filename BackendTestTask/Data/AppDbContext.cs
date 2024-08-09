using BackendTestTask.Models;
using Microsoft.EntityFrameworkCore;
using BackendTestTask.Data.Configurations;

namespace BackendTestTask.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<TreeNode> TreeNodes { get; set; }
    public DbSet<ExceptionLog> ExceptionLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TreeNodeConfiguration());
    }
}
