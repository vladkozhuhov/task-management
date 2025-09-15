using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

/// <summary>
/// Интерфейс контекста базы данных для слоя приложения
/// </summary>
public interface IApplicationDbContext
{
    DbSet<TaskItem> Tasks { get; }
    DbSet<User> Users { get; }
    DbSet<TaskRelation> TaskRelations { get; }
    
    /// <summary>
    /// Сохранить изменения в базе данных
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
