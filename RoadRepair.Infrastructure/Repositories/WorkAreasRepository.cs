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
