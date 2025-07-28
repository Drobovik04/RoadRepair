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
    public class ContractorServicesRepository : IContractorServiceRepository
    {
        private readonly AppDbContext _context;
        public ContractorServicesRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddContractorServiceAsync(ContractorService contractorService)
        {
            await _context.ContractorServices.AddAsync(contractorService);
        }
        public async Task<ContractorService?> GetContractorServiceByIdAsync(long contractorServiceId)
        {
            return await _context.ContractorServices.FirstOrDefaultAsync(x => x.Id == contractorServiceId);
        }
        public async Task<IEnumerable<ContractorService>> GetAllContractorServicesAsync()
        {
            return await _context.ContractorServices.ToListAsync();
        }
        public async Task<IEnumerable<ContractorService>> GetAllContractorServicesByWorkAreaIdAsync(long workAreaId)
        {
            return await _context.ContractorServices.Include(x => x.TypeOfService).Include(x => x.Contractor).Where(x => x.WorkAreaId == workAreaId).ToListAsync();
        }
        public void UpdateContractorService(ContractorService contractorService)
        {
            _context.ContractorServices.Update(contractorService);
        }

        public void DeleteContractorService(ContractorService contractorService)
        {
            _context.ContractorServices.Remove(contractorService);
        }
    }
}
