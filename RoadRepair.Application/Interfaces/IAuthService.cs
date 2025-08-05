using ErrorOr;
using Microsoft.AspNetCore.Identity;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ErrorOr<(bool, long)>> RegisterAsync(string lastName, string firstName, string? middleName, string userName, string email, string password);
        Task<ErrorOr<(string, string)>> LoginAsync(string email, string password);
        Task<ErrorOr<List<IdentityRole<long>>>> GetAllRoles();
        Task<ErrorOr<List<IdentityUser<long>>>> GetAllUsers();
        Task<ErrorOr<IdentityUser<long>>> GetIdentityUserById(long identityId);
        Task BlockUserById(long identityId, bool status);
        Task UpdateRoleForUser(long identityId, long roleId);
        Task UpdateIdentityUser(IdentityUser<long> identityUser);
        Task DeleteIdentityUser(long identityId);
        Task<ErrorOr<List<User>>> GetAllOrgUsers();
        Task<ErrorOr<long>> GetUserIdByUserName(string userName);
        Task<ErrorOr<bool>> LogoutAsync(string refreshToken);
        Task<ErrorOr<string>> RefreshTokenAsync(string refreshToken);
        Task<ErrorOr<Dictionary<string, object>>> GetInfoAboutUser(string id);
    }
}
