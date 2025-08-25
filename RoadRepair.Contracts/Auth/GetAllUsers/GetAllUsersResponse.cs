using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RoadRepair.Contracts.Auth.GetAllUsers
{
    public record UserInfo(long Id, string UserName, string? PhoneNumber, string LastName, string? MiddleName, string FirstName, string Email, bool IsBlocked, long RoleId, string RoleName, DateTime CreatedAt);
    public record GetAllUsersResponse();
}