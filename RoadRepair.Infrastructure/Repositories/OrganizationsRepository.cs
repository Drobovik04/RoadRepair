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
    public class OrganizationsRepository : IOrganizationRepository
    {
        private readonly AppDbContext _context;
        public OrganizationsRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddOrganizationAsync(Organization organization)
        {
            await _context.Organizations.AddAsync(organization);
        }

        public void DeleteOrganization(Organization organization)
        {
            _context.Organizations.Remove(organization);
        }

        public async Task<IEnumerable<Organization>> GetAllOrganizationsAsync()
        {
            return await _context.Organizations.ToListAsync();
        }
        public async Task<Organization?> GetOrganizationByIdAsync(long id)
        {
            return await _context.Organizations.FirstOrDefaultAsync(x => x.Id == id);
        }

        public void UpdateOrganization(Organization organization)
        {
            _context.Organizations.Update(organization);
        }
        // Реализация других методов
    }
}
