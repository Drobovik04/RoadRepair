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
    public class WorkTimesRepository : IWorkTimeRepository
    {
        private readonly AppDbContext _context;
        public WorkTimesRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddWorkTimeAsync(WorkTime workTime)
        {
            await _context.WorkTimes.AddAsync(workTime);
        }
        public async Task<WorkTime?> GetWorkTimeByIdAsync(long workTimeId)
        {
            return await _context.WorkTimes.FirstOrDefaultAsync(x => x.Id == workTimeId);
        }
        public async Task<IEnumerable<WorkTime>> GetAllWorkTimesAsync()
        {
            return await _context.WorkTimes.ToListAsync();
        }
        public void UpdateWorkTime(WorkTime workTime)
        {
            _context.WorkTimes.Update(workTime);
        }

        public void DeleteWorkTime(WorkTime workTime)
        {
            _context.WorkTimes.Remove(workTime);
        }

        public async Task<IEnumerable<WorkTime>> GetAllWorkTimesByRepairEventWorkerIdAsync(long repairEventWorkerId)
        {
            return await _context.WorkTimes.Where(x => x.RepairEventWorkerId == repairEventWorkerId).ToListAsync();
        }

        public void DeleteAllWorkTimes(List<WorkTime> workTimes)
        {
            _context.WorkTimes.RemoveRange(workTimes);
        }
    }
}
