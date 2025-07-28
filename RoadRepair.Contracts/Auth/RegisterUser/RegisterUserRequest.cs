using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.Auth.RegisterUser
{
    public record RegisterUserRequest(string LastName, string FirstName, string? MiddleName, string UserName, string Email, string Password, long OrganizationId);
}
