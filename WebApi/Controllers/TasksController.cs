using Application.Tasks.Commands;
using Application.Tasks.DTO;
using Application.Tasks.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

/// <summary>
/// Контроллер для работы с задачами
/// </summary>
[Authorize]
public class TasksController(IMediator mediator) : BaseApiController
{
    /// <summary>
    /// Получить список задач с фильтрацией
    /// </summary>
    /// <param name="query">Параметры запроса (статус, приоритет, включать ли подзадачи)</param>
    /// <returns>Список задач</returns>
    [HttpGet]
    public async Task<ActionResult<List<TaskItemDto>>> GetTasks([FromQuery] GetTasksQuery query)
    {
        var result = await mediator.Send(query);
        return Ok(result);
    }
    
    /// <summary>
    /// Получить задачу по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор задачи</param>
    /// <returns>Детальная информация о задаче</returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TaskItemDto>> GetTaskById(Guid id)
    {
        var result = await mediator.Send(new GetTaskByIdQuery { Id = id });
        return Ok(result);
    }
    
    /// <summary>
    /// Создать новую задачу
    /// </summary>
    /// <param name="command">Данные для создания задачи</param>
    /// <returns>Созданная задача</returns>
    [HttpPost]
    public async Task<ActionResult<TaskItemDto>> CreateTask(CreateTaskCommand command)
    {
        var result = await mediator.Send(command);
        return CreatedAtAction(nameof(GetTaskById), new { id = result.Id }, result);
    }
    
    /// <summary>
    /// Обновить существующую задачу
    /// </summary>
    /// <param name="id">Идентификатор задачи</param>
    /// <param name="command">Данные для обновления задачи</param>
    /// <returns>Обновленная задача</returns>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<TaskItemDto>> UpdateTask(Guid id, UpdateTaskCommand command)
    {
        if (id != command.Id)
            return BadRequest("Id в пути запроса не совпадает с Id в теле запроса");
        
        var result = await mediator.Send(command);
        return Ok(result);
    }
    
    /// <summary>
    /// Удалить задачу по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор задачи</param>
    /// <returns>Статус 204 (No Content) при успешном удалении</returns>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteTask(Guid id)
    {
        await mediator.Send(new DeleteTaskCommand { Id = id });
        return NoContent();
    }
}
