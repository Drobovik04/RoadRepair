using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.Auth.LoginUser
{
    public record LoginUserRequest(string EmailOrUserName, string Password);
}
