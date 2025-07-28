using Microsoft.AspNetCore.Identity;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Infrastructure.Identity
{
    public class AppUser: IdentityUser<long>
    {
        //public string LastName { get; set; }
        //public string FirstName { get; set; }
        //public string? MiddleName { get; set; }
        //public long? OrganizationId { get; set; }
        //public Organization? Organization { get; set; }
        //public long? RoleId {  get; set; }
        //public Role? Role { get; set; }
        //public DateTime CreatedAt { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
