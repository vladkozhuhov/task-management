using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Tasks.DTO;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tasks.Queries;

public class GetTaskByIdQuery : IRequest<TaskItemDto>
{
    public Guid Id { get; set; }
}

public class GetTaskByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetTaskByIdQuery, TaskItemDto>
{
    public async Task<TaskItemDto> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var task = await context.Tasks
            .Include(t => t.Author)
            .Include(t => t.Assignee)
            .Include(t => t.ParentTask)
            .Include(t => t.SubTasks)
            .Include(t => t.RelatedFromTasks)
                .ThenInclude(r => r.TargetTask)
            .Include(t => t.RelatedToTasks)
                .ThenInclude(r => r.SourceTask)
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (task == null)
            throw new NotFoundException(nameof(TaskItem), request.Id);

        return mapper.Map<TaskItemDto>(task);
    }
}
