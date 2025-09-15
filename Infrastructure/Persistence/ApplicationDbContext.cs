using System.Reflection;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    public DbSet<TaskItem> Tasks => Set<TaskItem>();
    public DbSet<User> Users => Set<User>();
    public DbSet<TaskRelation> TaskRelations => Set<TaskRelation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        // Настройка отношений для задач
        modelBuilder.Entity<TaskItem>()
            .HasOne(t => t.Author)
            .WithMany(u => u.CreatedTasks)
            .HasForeignKey(t => t.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<TaskItem>()
            .HasOne(t => t.Assignee)
            .WithMany(u => u.AssignedTasks)
            .HasForeignKey(t => t.AssigneeId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
        
        modelBuilder.Entity<TaskItem>()
            .HasOne(t => t.ParentTask)
            .WithMany(t => t.SubTasks)
            .HasForeignKey(t => t.ParentTaskId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Настройка отношений для связей между задачами
        modelBuilder.Entity<TaskRelation>()
            .HasOne(r => r.SourceTask)
            .WithMany(t => t.RelatedFromTasks)
            .HasForeignKey(r => r.SourceTaskId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<TaskRelation>()
            .HasOne(r => r.TargetTask)
            .WithMany(t => t.RelatedToTasks)
            .HasForeignKey(r => r.TargetTaskId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
