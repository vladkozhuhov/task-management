using Application.Auth.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

/// <summary>
/// Контроллер для аутентификации пользователей
/// </summary>
public class AuthController(IMediator mediator) : BaseApiController
{
    /// <summary>
    /// Аутентификация пользователя
    /// </summary>
    /// <param name="command">Команда входа с email и паролем</param>
    /// <returns>JWT токен и информация о пользователе</returns>
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }
}
