using Microsoft.EntityFrameworkCore;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Domain.Entities;
using RoadRepair.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Infrastructure.Repositories
{
    public class UsersRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UsersRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<long> AddNewUserAsync(User user)
        {
            await _context.OrgUsers.AddAsync(user);
            return user.Id;
        }
        public async Task<User?> FindUserByIdAsync(long userId)
        {
            return await _context.OrgUsers.FirstOrDefaultAsync(x => x.Id == userId);
        }
        public async Task<User?> FindUserByAppUserIdAsync(long identityId)
        {
            return await _context.OrgUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
        }
        public void UpdateUser(User user)
        {
            _context.OrgUsers.Update(user);
        }

        public void DeleteUser(User user)
        {
            _context.OrgUsers.Remove(user);
        }

    }
}
