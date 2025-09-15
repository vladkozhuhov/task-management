using Application.Common.Interfaces;
using Application.Tasks.DTO;
using AutoMapper;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskStatus = Domain.Enums.TaskStatus;

namespace Application.Tasks.Queries;

public class GetTasksQuery : IRequest<List<TaskItemDto>>
{
    public string? Status { get; set; }
    public string? Priority { get; set; }
    public bool IncludeSubtasks { get; set; }
}

public class GetTasksQueryHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetTasksQuery, List<TaskItemDto>>
{
    public async Task<List<TaskItemDto>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
    {
        var query = context.Tasks
            .Include(t => t.Author)
            .Include(t => t.Assignee)
            .AsQueryable();

        if (request.Status != null)
        {
            if (Enum.TryParse<TaskStatus>(request.Status, out var status))
            {
                query = query.Where(t => t.Status == status);
            }
        }

        if (request.Priority != null)
        {
            if (Enum.TryParse<Priority>(request.Priority, out var priority))
            {
                query = query.Where(t => t.Priority == priority);
            }
        }

        // Фильтруем только верхнеуровневые задачи, если не требуется включать подзадачи
        if (!request.IncludeSubtasks)
        {
            query = query.Where(t => t.ParentTaskId == null);
        }

        var tasks = await query.ToListAsync(cancellationToken);

        return mapper.Map<List<TaskItemDto>>(tasks);
    }
}
