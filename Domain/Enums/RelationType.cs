namespace Domain.Enums;

/// <summary>
/// Тип связи между задачами
/// </summary>
public enum RelationType
{
    /// <summary>
    /// Задачи связаны между собой
    /// </summary>
    RelatedTo,
    
    /// <summary>
    /// Задача заблокирована другой задачей
    /// </summary>
    BlockedBy,
    
    /// <summary>
    /// Задача дублирует другую задачу
    /// </summary>
    Duplicates,
    
    /// <summary>
    /// Задача является продолжением другой задачи
    /// </summary>
    Continues,
    
    /// <summary>
    /// Задача исправляет ошибку в другой задаче
    /// </summary>
    Fixes
}
