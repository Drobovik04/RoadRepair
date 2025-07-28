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
    public class RepairEventMediaRepository : IRepairEventMediaRepository
    {
        private readonly AppDbContext _context;
        public RepairEventMediaRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddRepairEventMediaAsync(RepairEventMedia repairEventMedia)
        {
            await _context.RepairEventMedia.AddAsync(repairEventMedia);
        }
        public async Task<RepairEventMedia?> GetRepairEventMediaByIdAsync(long repairEventMediaId)
        {
            return await _context.RepairEventMedia.FirstOrDefaultAsync(x => x.Id == repairEventMediaId);
        }
        public async Task<IEnumerable<RepairEventMedia>> GetAllRepairEventMediaAsync()
        {
            return await _context.RepairEventMedia.ToListAsync();
        }
        public async Task<IEnumerable<RepairEventMedia>> GetAllRepairEventMediaByRepairZoneIdAsync(long repairEventId)
        {
            return await _context.RepairEventMedia.Where(x => x.RepairEventId == repairEventId).ToListAsync();
        }
        public void UpdateRepairEventMedia(RepairEventMedia repairEventMedia)
        {
            _context.RepairEventMedia.Update(repairEventMedia);
        }
        public void DeleteRepairEventMedia(RepairEventMedia repairEventMedia)
        {
            _context.RepairEventMedia.Remove(repairEventMedia);
        }

    }
}
