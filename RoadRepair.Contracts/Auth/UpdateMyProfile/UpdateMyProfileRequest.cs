using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.Auth.UpdateMyProfile
{
    public record UpdateMyProfileRequest(string UserName, string? PhoneNumber, string LastName, string? MiddleName, string FirstName, string Email);
}
