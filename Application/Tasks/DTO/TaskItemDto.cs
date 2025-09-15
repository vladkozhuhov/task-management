namespace Application.Tasks.DTO;

/// <summary>
/// DTO для передачи информации о задаче в API
/// </summary>
public class TaskItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid AuthorId { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public Guid? AssigneeId { get; set; }
    public string? AssigneeName { get; set; }
    public Guid? ParentTaskId { get; set; }
    public string? ParentTaskTitle { get; set; }
    public ICollection<SubTaskDto> SubTasks { get; set; } = new List<SubTaskDto>();
    public ICollection<TaskRelationDto> Relations { get; set; } = new List<TaskRelationDto>();
    
    // Вычисляемые свойства для расширенной функциональности
    public int CompletionPercentage 
    { 
        get 
        {
            if (!SubTasks.Any()) return Status == "Done" ? 100 : 0;
            return (int)Math.Round(SubTasks.Count(t => t.Status == "Done") / (double)SubTasks.Count * 100);
        } 
    }
    
    public bool IsOverdue => DueDate.HasValue && DueDate.Value < DateTime.UtcNow && Status != "Done";
    
    public TimeSpan? TimeToComplete => Status == "Done" && UpdatedAt.HasValue 
        ? UpdatedAt.Value - CreatedAt 
        : null;
}