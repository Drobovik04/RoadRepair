using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RoadRepair.Application.Interfaces;
using RoadRepair.Application.Organizations.Queries.GetOrganization;
using RoadRepair.Application.Users.Commands.AssignIdentityAndUser;
using RoadRepair.Application.Users.Commands.CreateUser;
using RoadRepair.Contracts.Auth.LoginUser;
using RoadRepair.Contracts.Auth.RegisterUser;
using RoadRepair.Domain.Entities;
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

        public AuthController(IAuthService authService, IMediator mediator)
        {
            _authService = authService;
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            var result = await _authService.RegisterAsync(request.LastName, request.FirstName, request.MiddleName, request.UserName, request.Email, request.Password);

            if (result.IsError)
            {
                return Problem(detail: result.FirstError.Description);
            }

            var createOrgUserCommand = new CreateUserCommand(request.LastName, request.MiddleName, request.FirstName, request.OrganizationId, result.Value.Item2);
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
