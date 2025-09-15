namespace Application.Tasks.DTO;

/// <summary>
/// DTO для представления связей между задачами
/// </summary>
public class TaskRelationDto
{
    public Guid RelatedTaskId { get; set; }
    public string RelatedTaskTitle { get; set; } = string.Empty;
    public string RelationType { get; set; } = string.Empty;
    public string RelatedTaskStatus { get; set; } = string.Empty;
}