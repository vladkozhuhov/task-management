using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user, List<string>? roles = null);
    bool ValidateToken(string token);
    Guid GetUserIdFromToken(string token);
}
