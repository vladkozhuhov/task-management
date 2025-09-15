using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Tasks.Commands;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests.Application.Commands;

/// <summary>
/// Тесты для команды создания задачи
/// </summary>
public class CreateTaskCommandTests
{
    /// <summary>
    /// Проверяет успешное создание задачи при валидной команде
    /// </summary>
    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateTask()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        var dbContext = new ApplicationDbContext(options);
        
        var mockCurrentUserService = new Mock<ICurrentUserService>();
        var userId = Guid.NewGuid();
        mockCurrentUserService.Setup(m => m.UserId).Returns(userId);
        
        var user = new User
        {
            Id = userId,
            UserName = "testuser",
            Email = "test@example.com"
        };
        
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        var mapper = mockMapper.CreateMapper();
        
        var handler = new CreateTaskCommandHandler(dbContext, mockCurrentUserService.Object, mapper);
        
        var command = new CreateTaskCommand
        {
            Title = "Test Task",
            Description = "Test Description",
            Priority = "Medium",
            DueDate = DateTime.UtcNow.AddDays(1)
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be(command.Title);
        result.Description.Should().Be(command.Description);
        result.Priority.Should().Be(command.Priority);
        result.Status.Should().Be("New");
        result.AuthorId.Should().Be(userId);
        
        var taskInDb = await dbContext.Tasks.FindAsync(result.Id);
        taskInDb.Should().NotBeNull();
        taskInDb!.Title.Should().Be(command.Title);
    }
    
    /// <summary>
    /// Проверяет выбрасывание исключения при попытке создать задачу неавторизованным пользователем
    /// </summary>
    [Fact]
    public async Task Handle_UnauthenticatedUser_ShouldThrowUnauthorizedException()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        var dbContext = new ApplicationDbContext(options);
        
        var mockCurrentUserService = new Mock<ICurrentUserService>();
        mockCurrentUserService.Setup(m => m.UserId).Returns((Guid?)null);
        
        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        var mapper = mockMapper.CreateMapper();
        
        var handler = new CreateTaskCommandHandler(dbContext, mockCurrentUserService.Object, mapper);
        
        var command = new CreateTaskCommand
        {
            Title = "Test Task",
            Description = "Test Description",
            Priority = "Medium"
        };

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            async () => await handler.Handle(command, CancellationToken.None)
        );
    }
}
