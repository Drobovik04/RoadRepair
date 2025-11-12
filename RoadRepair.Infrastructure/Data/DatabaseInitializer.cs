using Microsoft.AspNetCore.Identity;
using RoadRepair.Domain.Entities;
using RoadRepair.Infrastructure.Identity;
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

		public static async Task SeedAdminUserAsync(
			UserManager<AppUser> userManager,
			RoleManager<IdentityRole<long>> roleManager,
			AppDbContext dbContext,
			string adminUserName = "Test11",
			string adminEmail = "test11@mail.ru",
			string adminPassword = "123456",
			string lastName = "Сосновский",
			string firstName = "Кирилл",
			string middleName = "Евгеньевич")
		{
			// Ensure Admin role exists
			if (!await roleManager.RoleExistsAsync("Admin"))
			{
				await roleManager.CreateAsync(new IdentityRole<long>("Admin"));
			}

			// Find existing admin by username or email
			var existingByName = await userManager.FindByNameAsync(adminUserName);
			var existingByEmail = await userManager.FindByEmailAsync(adminEmail);
			var adminUser = existingByName ?? existingByEmail;

			if (adminUser == null)
			{
				adminUser = new AppUser
				{
					UserName = adminUserName,
					Email = adminEmail,
					EmailConfirmed = false,
					PhoneNumber = "+375291234567",
					PhoneNumberConfirmed = false,
					IsBlocked = false
				};

				var createResult = await userManager.CreateAsync(adminUser, adminPassword);
				if (!createResult.Succeeded)
				{
					throw new InvalidOperationException("Failed to create default admin user: " + string.Join(", ", createResult.Errors.Select(e => e.Description)));
				}
			}

			// Ensure user is in Admin role
			if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
			{
				await userManager.AddToRoleAsync(adminUser, "Admin");
			}

			// Ensure related profile exists in UsersInfo
			var existingProfile = dbContext.UsersInfo.FirstOrDefault(x => x.IdentityId == adminUser.Id);
			if (existingProfile == null)
			{
				var profile = new User(lastName, middleName, firstName, adminUser.Id)
				{
					CreatedAt = DateTime.UtcNow
				};
				await dbContext.UsersInfo.AddAsync(profile);
				await dbContext.CommitChangesAsync();
			}
		}
	}
}
