using Backend.Application.DTOs.Identity;
using Backend.Application.DTOs.Results;
using System.Threading.Tasks;

namespace Backend.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<Result<TokenResponse>> GetTokenAsync(TokenRequest request, string ipAddress);

        Task<Result<TokenResponse>> RefreshTokenAsync(string userId, string ipAddress);

        Task<Result<string>> RegisterAsync(RegisterRequest request, string origin);


        Task ForgotPassword(ForgotPasswordRequest model, string origin);

        Task<Result<string>> ResetPassword(ResetPasswordRequest model);

    }
}