using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class TaskRelationConfiguration : IEntityTypeConfiguration<TaskRelation>
{
    [Obsolete("Obsolete")]
    public void Configure(EntityTypeBuilder<TaskRelation> builder)
    {
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.RelationType)
            .IsRequired()
            .HasConversion<string>();
        
        // Запрещаем связь задачи самой с собой
        builder.HasCheckConstraint("CK_TaskRelation_NoSelfRelation", "\"SourceTaskId\" <> \"TargetTaskId\"");
    }
}
