using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Persistence.Repositories;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    private IRepository<TaskItem>? _taskRepository;
    private IRepository<User>? _userRepository;
    private IRepository<TaskRelation>? _taskRelationRepository;

    public IRepository<TaskItem> TaskRepository => 
        _taskRepository ??= new Repository<TaskItem>(context);

    public IRepository<User> UserRepository => 
        _userRepository ??= new Repository<User>(context);

    public IRepository<TaskRelation> TaskRelationRepository => 
        _taskRelationRepository ??= new Repository<TaskRelation>(context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }
}
