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
    public class RepairEventWorkersRepository : IRepairEventWorkerRepository
    {
        private readonly AppDbContext _context;
        public RepairEventWorkersRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddRepairEventWorkerAsync(RepairEventWorker repairEventWorker)
        {
            await _context.RepairEventWorkers.AddAsync(repairEventWorker);
        }
        public async Task<RepairEventWorker?> GetRepairEventWorkerByIdAsync(long repairEventWorkerId)
        {
            return await _context.RepairEventWorkers.FirstOrDefaultAsync(x => x.Id == repairEventWorkerId);
        }
        public async Task<IEnumerable<RepairEventWorker>> GetAllRepairEventWorkersAsync()
        {
            return await _context.RepairEventWorkers.ToListAsync();
        }
        public void UpdateRepairEventWorker(RepairEventWorker repairEventWorker)
        {
            _context.RepairEventWorkers.Update(repairEventWorker);
        }

        public void DeleteRepairEventWorker(RepairEventWorker repairEventWorker)
        {
            _context.RepairEventWorkers.Remove(repairEventWorker);
        }

        public async Task<IEnumerable<RepairEventWorker>> GetRepairEventWorkerByRepairEventIdAsync(long repairEventId)
        {
            return await _context.RepairEventWorkers.Where(x => x.RepairEventId == repairEventId).ToListAsync();
        }
    }
}
