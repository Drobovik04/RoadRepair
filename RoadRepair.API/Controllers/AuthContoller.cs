using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RoadRepair.Application.Interfaces;
using RoadRepair.Application.Materials.Commands.UpdateMaterial;
using RoadRepair.Application.Users.Commands.AssignIdentityAndUser;
using RoadRepair.Application.Users.Commands.CreateUser;
using RoadRepair.Application.Users.Commands.DeleteUser;
using RoadRepair.Application.Users.Commands.ToggleBlockUser;
using RoadRepair.Application.Users.Commands.UpdateUser;
using RoadRepair.Contracts.Auth.GetAllUsers;
using RoadRepair.Contracts.Auth.LoginUser;
using RoadRepair.Contracts.Auth.RegisterUser;
using RoadRepair.Contracts.Auth.ToggleBlockUser;
using RoadRepair.Contracts.Auth.UpdateUser;
using RoadRepair.Contracts.Materials.UpdateMaterial;
using RoadRepair.Domain.Entities;
using RoadRepair.Infrastructure.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RoadRepair.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private IMediator _mediator;
        private UserManager<AppUser> _userManager;
        private RoleManager<IdentityRole<long>> _roleManager;

        public AuthController(IAuthService authService, IMediator mediator, UserManager<AppUser> userManager, RoleManager<IdentityRole<long>> roleManager)
        {
            _authService = authService;
            _mediator = mediator;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            var result = await _authService.RegisterAsync(request.LastName, request.FirstName, request.MiddleName, request.UserName, request.Email, request.Password);

            if (result.IsError)
            {
                return Problem(detail: result.FirstError.Description);
            }

            var createOrgUserCommand = new CreateUserCommand(request.LastName, request.MiddleName, request.FirstName, result.Value.Item2);
            var orgUser = await _mediator.Send(createOrgUserCommand);

            if (orgUser.IsError)
            {
                return Problem(detail: orgUser.FirstError.Description);
            }

            var assignCommand = new AssignIdentityAndUserCommand(orgUser.Value.Id, _authService.GetUserIdByUserName(request.UserName).Result.Value);
            var assignResult = await _mediator.Send(assignCommand);

            return assignResult.MatchFirst(
                res => Ok(res),
                error => Problem(
                    detail: error.Description));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
        {
            var result = await _authService.LoginAsync(request.EmailOrUserName, request.Password);
            return result.MatchFirst(
                res => {
                    SetRefreshCookie(res.Item2);
                    return Ok(new { token = res.Item1 });
                    },
                error => Problem(
                    detail: error.Description));
        }

        [HttpGet("getAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await _authService.GetAllRoles();
            return result.MatchFirst(
                result => Ok(result), 
                error => Problem(detail: error.Description));
        }

        [HttpGet("getAllUsers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _authService.GetAllUsers();

            var result = new List<UserInfo>();

            var orgUsers = await _authService.GetAllOrgUsers();

            if (!users.IsError && !orgUsers.IsError)
            {
                var orgUsersValue = orgUsers.Value;
                foreach(var user in users.Value)
                {
                    var orgUserAssignWithIdentity = orgUsersValue.FirstOrDefault(x => x.IdentityId == user.Id);
                    var roles = await _userManager.GetRolesAsync(user as AppUser);
                    var role = roles.Contains("Admin") ? "Admin" : "User";
                    var roleId = _roleManager.Roles.FirstOrDefault(x => x.Name == role).Id;
                    var sadsad = (user as AppUser).IsBlocked;
                    result.Add(new UserInfo(orgUserAssignWithIdentity.Id, user.UserName, user.PhoneNumber, orgUserAssignWithIdentity.LastName, orgUserAssignWithIdentity.MiddleName, orgUserAssignWithIdentity.FirstName, user.Email, (user as AppUser).IsBlocked, roleId, role, orgUserAssignWithIdentity.CreatedAt));
                }
            }


            return Ok(result);
        }

        [HttpPut("updateUser/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(long id, [FromBody] UpdateUserRequest request)
        {
            var command = new UpdateUserCommand(id, request.UserName, request.PhoneNumber, request.LastName, request.MiddleName, request.FirstName, request.Email, request.RoleId);

            var result = await _mediator.Send(command);

            return result.MatchFirst(
                _ => Ok(null),
                error => Problem(error.Description));
        }

        [HttpPut("toggleBlockUser/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ToggleBlockUser(long id, [FromBody] ToggleBlockUserRequest request)
        {
            var command = new ToggleBlockUserCommand(id, request.Status);

            var result = await _mediator.Send(command);

            return result.MatchFirst(
                _ => Ok(null),
                error => Problem(error.Description));
        }

        [HttpDelete("deleteUser/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            var command = new DeleteUserCommand(id);

            var result = await _mediator.Send(command);

            return result.MatchFirst(
                _ => Ok(null),
                error => Problem(error.Description));
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var refresh = Request.Cookies["refreshToken"];
            if (refresh == null) 
            {
                return NotFound("Not found refreshToken");
            }
            var result = await _authService.RefreshTokenAsync(refresh);

            return result.MatchFirst(
                res => {
                    return Ok(new { token = res });
                },
                error => Problem(
                    detail: error.Description));

        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refresh = Request.Cookies["refreshToken"];
            if (refresh == null)
            {
                return NotFound("Not found refreshToken");
            }
            var result = await _authService.LogoutAsync(refresh);

            return result.MatchFirst(
                res => {
                    Response.Cookies.Delete("refreshToken");
                    return Ok(null); 
                },
                error => Problem(
                    detail: error.Description));

            //await _signInManager.SignOutAsync();
        }

        private void SetRefreshCookie(string token)
        {
            Response.Cookies.Append("refreshToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });
        }

        [Authorize]
        [HttpGet("getInfoAboutUser")]
        public async Task<IActionResult> GetInfoAboutUser()
        {
            var result = await _authService.GetInfoAboutUser(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

            return result.MatchFirst(
                res => {
                    var camelCaseDict = new Dictionary<string, object>();
                    foreach (var kv in res)
                    {
                        camelCaseDict[ToCamelCase(kv.Key)] = kv.Value;
                    }
                    return Ok(camelCaseDict);
                },
                error => Problem(
                    detail: error.Description));
        }

        private static string ToCamelCase(string s)
        {
            if (string.IsNullOrEmpty(s) || char.IsLower(s[0]))
                return s;

            return char.ToLowerInvariant(s[0]) + s.Substring(1);
        }
    }
}
