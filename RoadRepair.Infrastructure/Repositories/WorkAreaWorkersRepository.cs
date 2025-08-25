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
    public class WorkAreaWorkersRepository : IWorkAreaWorkerRepository
    {
        private readonly AppDbContext _context;
        public WorkAreaWorkersRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddWorkAreaWorkerAsync(WorkAreaWorker workAreaWorker)
        {
            await _context.WorkAreaWorkers.AddAsync(workAreaWorker);
        }
        public async Task<WorkAreaWorker?> GetWorkAreaWorkerByIdAsync(long workAreaWorkerId)
        {
            return await _context.WorkAreaWorkers.FirstOrDefaultAsync(x => x.Id == workAreaWorkerId);
        }
        public async Task<IEnumerable<WorkAreaWorker>> GetWorkAreaWorkersByIdsWithRepairEventAsync(List<long> workAreaWorkerIds)
        {
            return await _context.WorkAreaWorkers
                .Include(x => x.WorkArea)
                    .ThenInclude(x => x.RepairZones)
                        .ThenInclude(x => x.RepairEvents)
                .Where(x => workAreaWorkerIds.Contains(x.Id)).ToListAsync();
        }
        public async Task<IEnumerable<WorkAreaWorker>> GetAllWorkAreaWorkersAsync()
        {
            return await _context.WorkAreaWorkers.ToListAsync();
        }
        public void UpdateWorkAreaWorker(WorkAreaWorker workAreaWorker)
        {
            _context.WorkAreaWorkers.Update(workAreaWorker);
        }

        public void DeleteWorkAreaWorker(WorkAreaWorker workAreaWorker)
        {
            _context.WorkAreaWorkers.Remove(workAreaWorker);
        }

        public async Task<IEnumerable<WorkAreaWorker>> GetWorkAreaWorkerByWorkAreaIdAsync(long workAreaId)
        {
            return await _context.WorkAreaWorkers.Where(x => x.WorkAreaId == workAreaId).ToListAsync();
        }
    }
}
