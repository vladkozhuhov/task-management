using Application.Common.Interfaces;
using Application.Tasks.DTO;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using TaskStatus = Domain.Enums.TaskStatus;

namespace Application.Tasks.Commands;

/// <summary>
/// Команда для создания новой задачи
/// </summary>
public class CreateTaskCommand : IRequest<TaskItemDto>
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Priority { get; set; } = "Medium";
    public DateTime? DueDate { get; set; }
    public Guid? AssigneeId { get; set; }
    public Guid? ParentTaskId { get; set; }
}

/// <summary>
/// Обработчик команды создания задачи
/// </summary>
public class CreateTaskCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUserService,
    IMapper mapper)
    : IRequestHandler<CreateTaskCommand, TaskItemDto>
{
    /// <summary>
    /// Обработка команды создания задачи
    /// </summary>
    /// <param name="request">Запрос на создание задачи</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>DTO созданной задачи</returns>
    public async Task<TaskItemDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (userId == null)
            throw new UnauthorizedAccessException("Пользователь не авторизован");

        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Status = TaskStatus.New,
            Priority = Enum.Parse<Priority>(request.Priority),
            CreatedAt = DateTime.UtcNow,
            DueDate = request.DueDate,
            AuthorId = userId.Value,
            AssigneeId = request.AssigneeId,
            ParentTaskId = request.ParentTaskId
        };

        await context.Tasks.AddAsync(task, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<TaskItemDto>(task);
    }
}
