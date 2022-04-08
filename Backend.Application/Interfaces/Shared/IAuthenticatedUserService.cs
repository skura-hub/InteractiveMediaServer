
namespace Backend.Application.Interfaces.Shared
{
    public interface IAuthenticatedUserService
    {
        string UserId { get; }
        public string Login { get; }
    }
}