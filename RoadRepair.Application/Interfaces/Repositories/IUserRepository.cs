using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public Task<long> AddNewUserAsync(User user);
        public Task<User?> FindUserByIdAsync(long userId);
        public Task<User?> FindUserByAppUserIdAsync(long identityId);
        public void UpdateUser(User user);
        public void DeleteUser(User user);
    }
}
