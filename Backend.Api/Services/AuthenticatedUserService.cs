using Backend.Application.Interfaces.Shared;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Backend.Api.Services
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue("uid");
        }

        public string UserId { get; }
        public string Login { get; }
    }
}