namespace Application.Tasks.DTO;

/// <summary>
/// DTO для представления подзадачи в родительской задаче
/// </summary>
public class SubTaskDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string? AssigneeName { get; set; }
    public DateTime? DueDate { get; set; }
    
    public bool IsOverdue => DueDate.HasValue && DueDate.Value < DateTime.UtcNow && Status != "Done";
}