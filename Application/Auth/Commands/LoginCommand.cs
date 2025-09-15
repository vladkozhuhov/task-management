using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Auth.Commands;

public class LoginCommand : IRequest<LoginResponse>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public Guid UserId { get; set; }
}

public class LoginCommandHandler(IApplicationDbContext context, IJwtService jwtService)
    : IRequestHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (user == null)
        {
            // Создал реального пользователя (в реальном приложении должна быть регистрация)
            user = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                UserName = request.Email.Split('@')[0]
            };

            await context.Users.AddAsync(user, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        var token = jwtService.GenerateToken(user);

        return new LoginResponse
        {
            Token = token,
            UserName = user.UserName,
            UserId = user.Id
        };
    }
}
