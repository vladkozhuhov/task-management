using Domain.Enums;
using TaskStatus = Domain.Enums.TaskStatus;

namespace Domain.Entities;

/// <summary>
/// Сущность задачи в системе управления задачами
/// </summary>
public class TaskItem
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TaskStatus Status { get; set; }
    public Priority Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DueDate { get; set; }
    
    // Связи
    public Guid AuthorId { get; set; }
    public User Author { get; set; } = null!;
    public Guid? AssigneeId { get; set; }
    public User? Assignee { get; set; }
    
    // Иерархические связи
    public Guid? ParentTaskId { get; set; }
    public TaskItem? ParentTask { get; set; }
    public ICollection<TaskItem> SubTasks { get; set; } = new List<TaskItem>();
    
    // Связи между задачами
    public ICollection<TaskRelation> RelatedFromTasks { get; set; } = new List<TaskRelation>();
    public ICollection<TaskRelation> RelatedToTasks { get; set; } = new List<TaskRelation>();
}
