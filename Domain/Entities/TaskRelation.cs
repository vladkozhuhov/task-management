using Domain.Enums;

namespace Domain.Entities;

/// <summary>
/// Сущность, представляющая связь между задачами
/// </summary>
public class TaskRelation
{
    public Guid Id { get; set; }
    
    public Guid SourceTaskId { get; set; }
    public TaskItem SourceTask { get; set; } = null!;
    
    public Guid TargetTaskId { get; set; }
    public TaskItem TargetTask { get; set; } = null!;
    
    public RelationType RelationType { get; set; }
}
