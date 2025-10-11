using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Infrastructure.Data
{
    public static class DatabaseInitializer
    {
        private static readonly string[] Roles = new[] { "Admin", "User", "Manager" };

        public static async Task SeedRolesAsync(RoleManager<IdentityRole<long>> roleManager)
        {
            foreach (var roleName in Roles)
            {
                var roleExists = await roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole<long>(roleName));
                }
            }
        }
    }
}
