using Azure;
using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole<long>> roleManager, IUserRepository userRepository, IConfiguration configuration, AppDbContext appDbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userRepository = userRepository;
            _configuration = configuration;
            _appDbContext = appDbContext;
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

        public async Task<ErrorOr<List<IdentityRole<long>>>> GetAllRoles()
        {
            var list = await _roleManager.Roles.ToListAsync();

            if (list.Count != 0)
            {
                return list;
            }
            else
            {
                return Error.Failure(description: "There are not roles");
            }
        }

        public async Task<ErrorOr<List<IdentityUser<long>>>> GetAllUsers()
        {
            var list = await _userManager.Users.Select(x => x as IdentityUser<long>).ToListAsync();

            if (list.Count != 0)
            {
                return list;
            }
            else
            {
                return Error.Failure(description: "There are not users");
            }
        }

        public async Task<ErrorOr<List<User>>> GetAllOrgUsers()
        {
            var list = await _appDbContext.OrgUsers.ToListAsync();

            if (list.Count != 0)
            {
                return list;
            }
            else
            {
                return Error.Failure(description: "There are not orgUsers");
            }
        }

        public async Task<ErrorOr<IdentityUser<long>>> GetIdentityUserById(long id)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user != null)
            {
                return user;
            }
            else
            {
                return Error.Failure(description: "There is no user with such Id");
            }
        }

        public async Task BlockUserById(long identityId, bool status)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == identityId);

            if (user.IsBlocked != status)
            {
                user.IsBlocked = status;

                await _appDbContext.CommitChangesAsync();
            }
        }

        public async Task UpdateRoleForUser(long identityId, long roleId)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == identityId);
            var newRole = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == roleId);

            var roles = _roleManager.Roles;
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, newRole.Name);

        }

        public async Task UpdateIdentityUser(IdentityUser<long> identityUser)
        {
            _appDbContext.Users.Update(identityUser as AppUser);
            await _appDbContext.CommitChangesAsync();
        }

        public async Task DeleteIdentityUser(long identityId)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == identityId);
            if (user != null)
            {
                _appDbContext.Users.Remove(user);
                await _appDbContext.CommitChangesAsync();
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

            if (user.IsBlocked)
            {
                return Error.Failure(description: "This User is blocked");
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
            var identityUser = await _userManager.FindByIdAsync(id);
            var user = await _userRepository.FindUserByAppUserIdAsync(identityUser.Id);

            var info = new Dictionary<string, object>
            {
                { "OrganizationId", user.OrganizationId },
                { "LastName", user.LastName },
                { "FirstName", user.FirstName },
                { "MiddleName", user.MiddleName },
            };
            var roles = _userManager.GetRolesAsync(identityUser).Result.Select(x => new Claim("role", x));

            info.Add(roles.First().Type, roles.First().Value);

            info.Add("isBlocked", identityUser.IsBlocked);


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
            claims.AddRange(_userManager.GetRolesAsync(user).Result.Select(x => new Claim("role", x))); //это для того, чтобы считывать из поля Role, верхняя для ASP нормально работает

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
