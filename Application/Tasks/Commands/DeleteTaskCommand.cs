using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tasks.Commands;

public class DeleteTaskCommand : IRequest
{
    public Guid Id { get; set; }
}

public class DeleteTaskCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    : IRequestHandler<DeleteTaskCommand>
{
    public async Task Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (userId == null)
            throw new UnauthorizedAccessException("Пользователь не авторизован");

        var task = await context.Tasks
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
        
        if (task == null)
            throw new NotFoundException(nameof(TaskItem), request.Id);
        
        context.Tasks.Remove(task);
        await context.SaveChangesAsync(cancellationToken);
    }
}
