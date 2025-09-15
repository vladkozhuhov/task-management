using Application.Tasks.DTO;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TaskItem, TaskItemDto>()
            .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.Priority, opt => opt.MapFrom(s => s.Priority.ToString()))
            .ForMember(d => d.AuthorName, opt => opt.MapFrom(s => s.Author != null ? s.Author.UserName : string.Empty))
            .ForMember(d => d.AssigneeName, opt => opt.MapFrom(s => s.Assignee != null ? s.Assignee.UserName : null))
            .ForMember(d => d.ParentTaskTitle, opt => opt.MapFrom(s => s.ParentTask != null ? s.ParentTask.Title : null))
            .ForMember(d => d.SubTasks, opt => opt.MapFrom(s => s.SubTasks))
            .ForMember(d => d.Relations, opt => opt.MapFrom<TaskRelationsResolver>());

        CreateMap<TaskItem, SubTaskDto>()
            .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.Priority, opt => opt.MapFrom(s => s.Priority.ToString()))
            .ForMember(d => d.AssigneeName, opt => opt.MapFrom(s => s.Assignee != null ? s.Assignee.UserName : null));

        CreateMap<TaskRelation, TaskRelationDto>()
            .ForMember(d => d.RelationType, opt => opt.MapFrom(s => s.RelationType.ToString()))
            .ForMember(d => d.RelatedTaskId, opt => opt.MapFrom(s => s.TargetTaskId))
            .ForMember(d => d.RelatedTaskTitle, opt => opt.MapFrom(s => s.TargetTask.Title))
            .ForMember(d => d.RelatedTaskStatus, opt => opt.MapFrom(s => s.TargetTask.Status.ToString()));
    }
}

public abstract class TaskRelationsResolver : IValueResolver<TaskItem, TaskItemDto, ICollection<TaskRelationDto>>
{
    public ICollection<TaskRelationDto> Resolve(TaskItem source, TaskItemDto destination, ICollection<TaskRelationDto> destMember, ResolutionContext context)
    {
        var relations = new List<TaskRelationDto>();
        
        // Связи, где данная задача является исходной
        foreach (var relation in source.RelatedFromTasks)
        {
            relations.Add(new TaskRelationDto
            {
                RelatedTaskId = relation.TargetTaskId,
                RelatedTaskTitle = relation.TargetTask?.Title ?? string.Empty,
                RelationType = relation.RelationType.ToString(),
                RelatedTaskStatus = relation.TargetTask?.Status.ToString() ?? string.Empty
            });
        }
        
        // Связи, где данная задача является целевой
        foreach (var relation in source.RelatedToTasks)
        {
            relations.Add(new TaskRelationDto
            {
                RelatedTaskId = relation.SourceTaskId,
                RelatedTaskTitle = relation.SourceTask?.Title ?? string.Empty,
                RelationType = relation.RelationType.ToString(),
                RelatedTaskStatus = relation.SourceTask?.Status.ToString() ?? string.Empty
            });
        }
        
        return relations;
    }
}