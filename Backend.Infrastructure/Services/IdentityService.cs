using Backend.Application.DTOs.Identity;
using Backend.Application.DTOs.Mail;
using Backend.Application.DTOs.Results;
using Backend.Application.DTOs.Settings;
using Backend.Application.Enums;
using Backend.Application.Exceptions;
using Backend.Application.Interfaces;
using Backend.Application.Interfaces.Repositories;
using Backend.Application.Interfaces.Shared;
using Backend.Domain.Entities.Catalog;
using Backend.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Identity.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JWTSettings _jwtSettings;
        private readonly IDateTimeService _dateTimeService;
        //private readonly IMailService _mailService;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IWipMediaRepository _wipMediaRepository;

        public IdentityService(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<JWTSettings> jwtSettings,
            IDateTimeService dateTimeService,
            SignInManager<User> signInManager,
            IWipMediaRepository wipMediaRepository,
            ApplicationDbContext applicationDbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
            _dateTimeService = dateTimeService;
            _signInManager = signInManager;
            _applicationDbContext = applicationDbContext;
            _wipMediaRepository = wipMediaRepository;
        }

        public async Task<Result<TokenResponse>> RefreshTokenAsync(string name, string ipAddress) {
            var user = await _userManager.FindByNameAsync(name);
            if (user == null)
                throw new Exception($"Your account was changed. Please relogin");
            return await GetTokenAsync(user, ipAddress);
        }

        public async Task<Result<TokenResponse>> GetTokenAsync(TokenRequest request, string ipAddress)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                throw new Exception($"No Accounts Registered with {request.Email}.");
            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
            if (!user.EmailConfirmed)
                throw new Exception($"Email is not confirmed for '{request.Email}'.");
            if (!user.IsActive)
                throw new Exception($"Account for '{request.Email}' is not active. Please contact the Administrator.");
            if (!result.Succeeded)
                throw new Exception($"Invalid Credentials for '{request.Email}'.");
            return await GetTokenAsync(user, ipAddress);
        }
        public async Task<Result<TokenResponse>> GetTokenAsync(User user, string ipAddress) {
            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user, ipAddress);
            var response = new TokenResponse
            {
                id = user.Id,
                jwtToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                issuedOn = jwtSecurityToken.ValidFrom.ToLocalTime(),
                expiresOn = jwtSecurityToken.ValidTo.ToLocalTime(),
                email = user.Email,
                userName = user.UserName
            };
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            response.roles = rolesList.ToList();
            response.isVerified = user.EmailConfirmed;
            var refreshToken = GenerateRefreshToken(ipAddress);
            response.RefreshToken = refreshToken.Token;
            return new Result<TokenResponse>(response);
        }


        private async Task<JwtSecurityToken> GenerateJWToken(User user, string ipAddress)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
                new Claim("ip", ipAddress)
            }
            .Union(userClaims)
            .Union(roleClaims);
            return JWTGeneration(claims);
        }

        private JwtSecurityToken JWTGeneration(IEnumerable<Claim> claims)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }

        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        private RefreshToken GenerateRefreshToken(string ipAddress)
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }

        public async Task<Result<string>> RegisterAsync(RegisterRequest request, string origin)
        {
            var date = DateTime.UtcNow;
            var userWithSameLogin = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameLogin != null)
            {
                throw new ApiException($"Login '{request.UserName}' is already taken.");
            }
            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail == null)
            {
                var user = new User
                {
                    Email = request.Email,
                    UserName = request.UserName,
                    IsActive = true,
                    EmailConfirmed = true // TODO
                };
                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Roles.Basic.ToString());
                    await _wipMediaRepository.AddStartingMediaDocument(date, user.Id);
                    await _applicationDbContext.SaveChangesAsync();
                    return new Result<string>(user.Id, message: $"User Registered.");
                }
                else
                {
                    throw new ApiException($"{ result.Errors.First().Description}");
                }
            }
            else
            {
                throw new ApiException($"Email {request.Email } is already registered.");
            }
        }

        public async Task ForgotPassword(ForgotPasswordRequest model, string origin)
        {
            //ToDo
            var account = await _userManager.FindByEmailAsync(model.Email);
            if (account == null) return;

            var code = await _userManager.GeneratePasswordResetTokenAsync(account);
            var route = "api/identity/reset-password/";
            var _enpointUri = new Uri(string.Concat($"{origin}/", route));
            var emailRequest = new MailRequest()
            {
                Body = $"You reset token is - {code}",
                To = model.Email,
                Subject = "Reset Password",
            };
            //await _mailService.SendAsync(emailRequest);
        }

        public async Task<Result<string>> ResetPassword(ResetPasswordRequest model)
        {
            var account = await _userManager.FindByEmailAsync(model.Email);
            if (account == null) throw new ApiException($"No Accounts Registered with {model.Email}.");
            var result = await _userManager.ResetPasswordAsync(account, model.Token, model.Password);
            if (result.Succeeded)
            {
                return new Result<string>(model.Email, message: $"Password Resetted.");
            }
            else
            {
                throw new ApiException($"Error occured while reseting the password.");
            }
        }
    }
}