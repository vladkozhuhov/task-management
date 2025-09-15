using Domain.Entities;

namespace Domain.Interfaces;

/// <summary>
/// Интерфейс единицы работы, обеспечивающий доступ к репозиториям и транзакционную поддержку
/// </summary>
public interface IUnitOfWork
{
    IRepository<TaskItem> TaskRepository { get; }
    IRepository<User> UserRepository { get; }
    IRepository<TaskRelation> TaskRelationRepository { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
