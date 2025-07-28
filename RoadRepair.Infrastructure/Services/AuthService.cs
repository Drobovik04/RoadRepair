using Azure;
using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RoadRepair.Application.Interfaces;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Domain.Entities;
using RoadRepair.Infrastructure.Data;
using RoadRepair.Infrastructure.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

namespace RoadRepair.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole<long>> _roleManager;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _appDbContext;

        public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole<long>> roleManager, IUserRepository userRepository, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<ErrorOr<(bool, long)>> RegisterAsync(string lastName, string firstName, string? middleName, string userName, string email, string password)
        {
            var user = new AppUser 
            { 
                Email = email, 
                UserName = userName,
            };
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded) 
            {
                if (!await _roleManager.RoleExistsAsync("User")) await _roleManager.CreateAsync(new IdentityRole<long>("User"));
                await _userManager.AddToRoleAsync(user, "User");
            }

            return result.Succeeded
                ? (result.Succeeded, _userManager.FindByNameAsync(userName).Result.Id)
                : Error.Failure(description: string.Join(", ", result.Errors.Select(x => x.Description).ToList()));

        }

        private bool IsValidEmail(string email)
        {
            try
            {
                MailAddress m = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        public async Task<ErrorOr<(string, string)>> LoginAsync(string emailOrUserName, string password)
        {
            AppUser? user = await _userManager.FindByEmailAsync(emailOrUserName);
            if (IsValidEmail(emailOrUserName))
            {
                user = await _userManager.FindByEmailAsync(emailOrUserName);
            }
            else
            {
                user = await _userManager.FindByNameAsync(emailOrUserName);
            }
            if (user == null)
            {
                return Error.Failure(description: "There is no user with such email");
            }
                
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded)
            {
                return Error.Failure(description: "Invalid password"); // лучше так не делать, а то мы говорим, что такая почта есть
            }

            var token = GenerateToken(user);
            var refreshToken = Guid.NewGuid().ToString();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:RefreshTokenExpirationTimeMinutes"]));
            await _userManager.UpdateAsync(user);
            return (token, refreshToken);
        }
        public async Task<ErrorOr<long>> GetUserIdByUserName(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return Error.Failure(description: "There is no user with such UserName");
            }

            return user.Id;
        }

        public async Task<ErrorOr<bool>> LogoutAsync(string refreshToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if (user != null)
            {
                user.RefreshToken = null;
                await _userManager.UpdateAsync(user);
                return true;
            }
            else
            {
                return Error.Failure(description: "Could not find a user with this refreshToken or it's expired");
            }

            //await _signInManager.SignOutAsync();
        }

        public async Task<ErrorOr<string>> RefreshTokenAsync(string refreshToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken && u.RefreshTokenExpiryTime > DateTime.UtcNow);
            if (user != null)
            {
                return GenerateToken(user);
            }
            else
            {
                return Error.Failure(description: "Could not find a user with this refreshToken or it's expired");
            }
        }
        public async Task<ErrorOr<Dictionary<string, object>>> GetInfoAboutUser(string id)
        {
            var user = await _userRepository.FindUserByAppUserIdAsync((await _userManager.FindByIdAsync(id)).Id);

            var info = new Dictionary<string, object>
            {
                { "OrganizationId", user.OrganizationId },
                { "LastName", user.LastName },
                { "FirstName", user.FirstName },
                { "MiddleName", user.MiddleName },
            };

            return info;
        }

        private string GenerateToken(AppUser user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            claims.AddRange(_userManager.GetRolesAsync(user).Result.Select(x => new Claim(ClaimTypes.Role, x)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:AccessTokenExpirationTimeMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
