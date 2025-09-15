using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.Title)
            .HasMaxLength(200)
            .IsRequired();
        
        builder.Property(t => t.Description)
            .HasMaxLength(2000);
        
        builder.Property(t => t.Status)
            .IsRequired()
            .HasConversion<string>();
        
        builder.Property(t => t.Priority)
            .IsRequired()
            .HasConversion<string>();
        
        builder.Property(t => t.CreatedAt)
            .IsRequired();
    }
}
