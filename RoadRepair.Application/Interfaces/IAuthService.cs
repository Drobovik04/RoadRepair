using ErrorOr;
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
        Task<ErrorOr<long>> GetUserIdByUserName(string userName);
        Task<ErrorOr<bool>> LogoutAsync(string refreshToken);
        Task<ErrorOr<string>> RefreshTokenAsync(string refreshToken);
        Task<ErrorOr<Dictionary<string, object>>> GetInfoAboutUser(string id);
    }
}
