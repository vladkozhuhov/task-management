namespace Domain.Enums;

/// <summary>
/// Статус выполнения задачи
/// </summary>
public enum TaskStatus
{
    /// <summary>
    /// Новая задача, работа еще не начата
    /// </summary>
    New,
    
    /// <summary>
    /// Задача в процессе выполнения
    /// </summary>
    InProgress,
    
    /// <summary>
    /// Задача выполнена
    /// </summary>
    Done
}
