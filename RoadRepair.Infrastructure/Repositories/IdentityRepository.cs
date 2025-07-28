using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Infrastructure.Repositories
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly AppDbContext _context;
        public IdentityRepository(AppDbContext context) 
        {
            _context = context;
        }

        public bool IsExists(long identityId)
        {
            return _context.Users.Any(x => x.Id == identityId);
        }
    }
}
