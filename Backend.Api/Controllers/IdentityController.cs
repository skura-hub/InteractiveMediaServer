using Backend.Application.DTOs.Identity;
using Backend.Application.DTOs.Results;
using Backend.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Backend.Api.Controllers
{
    [Route("api/identity")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            this._identityService = identityService;
        }

        /// <summary>
        /// Generates a JSON Web Token for a valid combination of emailId and password.
        /// </summary>
        /// <param name="tokenRequest"></param>
        /// <returns></returns>
        [HttpPost("token")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Result<TokenResponse>), 200)]
        public async Task<IActionResult> GetTokenAsync(TokenRequest tokenRequest)
        {
            var ipAddress = GenerateIPAddress();
            var token = await _identityService.GetTokenAsync(tokenRequest, ipAddress);
            return Ok(token);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _identityService.RegisterAsync(request, origin));
        }

        [ProducesResponseType(typeof(Result<TokenResponse>), 200)]
        [HttpPost("verifyToken")]
        public async Task<IActionResult> VerifyToken()
        {
            var name = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            string ipAddress = GenerateIPAddress();
            return Ok(await _identityService.RefreshTokenAsync(name, ipAddress));
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest model)
        {
            await _identityService.ForgotPassword(model, Request.Headers["origin"]);
            return Ok();
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
        {
            return Ok(await _identityService.ResetPassword(model));
        }

        private string GenerateIPAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}