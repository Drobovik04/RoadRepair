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
    public class ContractorsRepository : IContractorRepository
    {
        private readonly AppDbContext _context;
        public ContractorsRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddContractorAsync(Contractor contractor)
        {
            await _context.Contractors.AddAsync(contractor);
        }
        public async Task<Contractor?> GetContractorByIdAsync(long contractorId)
        {
            return await _context.Contractors.FirstOrDefaultAsync(x => x.Id == contractorId);
        }
        public async Task<IEnumerable<Contractor>> GetAllContractorsAsync()
        {
            return await _context.Contractors.ToListAsync();
        }
        public void UpdateContractor(Contractor contractor)
        {
            _context.Contractors.Update(contractor);
        }

        public void DeleteContractor(Contractor contractor)
        {
            _context.Contractors.Remove(contractor);
        }
    }
}
