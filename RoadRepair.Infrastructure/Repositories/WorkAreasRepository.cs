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
    public class WorkAreasRepository : IWorkAreaRepository
    {
        private readonly AppDbContext _context;
        public WorkAreasRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddWorkAreaAsync(WorkArea workArea)
        {
            await _context.WorkAreas.AddAsync(workArea);
        }
        public async Task<WorkArea?> GetWorkAreaByIdAsync(long workAreaId)
        {
            return await _context.WorkAreas.Include(x => x.Responsible).FirstOrDefaultAsync(x => x.Id == workAreaId);
        }
        public async Task<IEnumerable<WorkArea>> GetAllWorkAreasAsync()
        {
            return await _context.WorkAreas.Include(x => x.Responsible).ToListAsync();
        }
        public async Task<WorkArea> GetWorkAreaWithAllDependencies(long workAreaId)
        {
            return await _context.WorkAreas
                .Include(x => x.Responsible)
                .Include(x => x.RepairZones)
                    .ThenInclude(x => x.RepairEvents)
                        .ThenInclude(x => x.TypeOfRepair)
                .Include(x => x.RepairZones)
                    .ThenInclude(x => x.RepairEvents)
                        .ThenInclude(x => x.MaterialSpends)
                            .ThenInclude(x => x.Material)
                                .ThenInclude(x => x.TypeOfMeasure)
                 .Include(x => x.RepairZones)
                    .ThenInclude(x => x.RepairEvents)
                        .ThenInclude(x => x.MaterialSpends)
                            .ThenInclude(x => x.Contractor)
                .Include(x => x.ContractorServices)
                    .ThenInclude(x => x.TypeOfService)
                .Include(x => x.ContractorServices)
                    .ThenInclude(x => x.Contractor)
                .Include(x => x.WorkAreaWorkers)
                    .ThenInclude(x => x.Worker)
                .Include(x => x.WorkAreaWorkers)
                    .ThenInclude(x => x.Worker)
                        .ThenInclude(x => x.Position)
                .Include(x => x.WorkAreaWorkers)
                    .ThenInclude(x => x.WorkTimes)
                .Where(x => x.Id == workAreaId)
                .FirstOrDefaultAsync();
        }
        public void UpdateWorkArea(WorkArea workArea)
        {
            _context.WorkAreas.Update(workArea);
        }

        public void DeleteWorkArea(WorkArea workArea)
        {
            _context.WorkAreas.Remove(workArea);
        }
    }
}
