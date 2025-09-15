using System.Security.Claims;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Identity;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public Guid? UserId
    {
        get
        {
            var user = httpContextAccessor.HttpContext?.User;
            
            if (user == null || !user.Identity?.IsAuthenticated == true)
                return null;
            
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            
            return string.IsNullOrEmpty(userId) ? null : Guid.Parse(userId);
        }
    }
}
