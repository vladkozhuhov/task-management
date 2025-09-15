using Application.Common.Mappings;
using Application.Tasks.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using TaskStatus = Domain.Enums.TaskStatus;

namespace Tests.Application.Queries;

/// <summary>
/// Тесты для запроса получения списка задач
/// </summary>
public class GetTasksQueryTests
{
    /// <summary>
    /// Проверяет, что запрос возвращает все задачи при отсутствии фильтров
    /// </summary>
    [Fact]
    public async Task Handle_ReturnsAllTasks_WhenNoFiltersApplied()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        var dbContext = new ApplicationDbContext(options);
        
        var author = new User
        {
            Id = Guid.NewGuid(),
            UserName = "author",
            Email = "author@example.com"
        };
        
        var assignee = new User
        {
            Id = Guid.NewGuid(),
            UserName = "assignee",
            Email = "assignee@example.com"
        };
        
        dbContext.Users.AddRange(author, assignee);
        await dbContext.SaveChangesAsync();
        
        var tasks = new List<TaskItem>
        {
            new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = "Task 1",
                Description = "Description 1",
                Status = TaskStatus.New,
                Priority = Priority.Low,
                CreatedAt = DateTime.UtcNow,
                AuthorId = author.Id,
                Author = author
            },
            new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = "Task 2",
                Description = "Description 2",
                Status = TaskStatus.InProgress,
                Priority = Priority.Medium,
                CreatedAt = DateTime.UtcNow,
                AuthorId = author.Id,
                Author = author,
                AssigneeId = assignee.Id,
                Assignee = assignee
            },
            new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = "Task 3",
                Description = "Description 3",
                Status = TaskStatus.Done,
                Priority = Priority.High,
                CreatedAt = DateTime.UtcNow,
                AuthorId = author.Id,
                Author = author
            }
        };
        
        dbContext.Tasks.AddRange(tasks);
        await dbContext.SaveChangesAsync();
        
        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        var mapper = mockMapper.CreateMapper();
        
        var handler = new GetTasksQueryHandler(dbContext, mapper);
        var query = new GetTasksQuery();
        
        // Act
        var result = await handler.Handle(query, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(3);
    }
    
    [Theory]
    [InlineData("New", 1)]
    [InlineData("InProgress", 1)]
    [InlineData("Done", 1)]
    public async Task Handle_ReturnsFilteredTasks_WhenStatusFilterApplied(string status, int expectedCount)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        var dbContext = new ApplicationDbContext(options);
        
        var author = new User
        {
            Id = Guid.NewGuid(),
            UserName = "author",
            Email = "author@example.com"
        };
        
        dbContext.Users.Add(author);
        await dbContext.SaveChangesAsync();
        
        var tasks = new List<TaskItem>
        {
            new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = "Task 1",
                Description = "Description 1",
                Status = TaskStatus.New,
                Priority = Priority.Low,
                CreatedAt = DateTime.UtcNow,
                AuthorId = author.Id,
                Author = author
            },
            new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = "Task 2",
                Description = "Description 2",
                Status = TaskStatus.InProgress,
                Priority = Priority.Medium,
                CreatedAt = DateTime.UtcNow,
                AuthorId = author.Id,
                Author = author
            },
            new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = "Task 3",
                Description = "Description 3",
                Status = TaskStatus.Done,
                Priority = Priority.High,
                CreatedAt = DateTime.UtcNow,
                AuthorId = author.Id,
                Author = author
            }
        };
        
        dbContext.Tasks.AddRange(tasks);
        await dbContext.SaveChangesAsync();
        
        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        var mapper = mockMapper.CreateMapper();
        
        var handler = new GetTasksQueryHandler(dbContext, mapper);
        var query = new GetTasksQuery
        {
            Status = status
        };
        
        // Act
        var result = await handler.Handle(query, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(expectedCount);
        result.All(t => t.Status == status).Should().BeTrue();
    }
}
