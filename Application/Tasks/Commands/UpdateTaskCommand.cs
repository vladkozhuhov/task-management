using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Tasks.DTO;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskStatus = Domain.Enums.TaskStatus;

namespace Application.Tasks.Commands;

public class UpdateTaskCommand : IRequest<TaskItemDto>
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public string? Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid? AssigneeId { get; set; }
}

public class UpdateTaskCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUserService,
    IMapper mapper)
    : IRequestHandler<UpdateTaskCommand, TaskItemDto>
{
    public async Task<TaskItemDto> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (userId == null)
            throw new UnauthorizedAccessException("Пользователь не авторизован");

        var task = await context.Tasks
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
        
        if (task == null)
            throw new NotFoundException(nameof(TaskItem), request.Id);
        
        if (request.Title != null)
            task.Title = request.Title;
        
        if (request.Description != null)
            task.Description = request.Description;
        
        if (request.Status != null && Enum.TryParse<TaskStatus>(request.Status, out var status))
            task.Status = status;
        
        if (request.Priority != null && Enum.TryParse<Priority>(request.Priority, out var priority))
            task.Priority = priority;
        
        task.DueDate = request.DueDate;
        task.AssigneeId = request.AssigneeId;
        task.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<TaskItemDto>(task);
    }
}
