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
    public class RepairZonesRepository : IRepairZoneRepository
    {
        private readonly AppDbContext _context;
        public RepairZonesRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddRepairZoneAsync(RepairZone repairZone)
        {
            await _context.RepairZones.AddAsync(repairZone);
        }
        public async Task<RepairZone?> GetRepairZoneByIdAsync(long repairZoneId)
        {
            return await _context.RepairZones.FirstOrDefaultAsync(x => x.Id == repairZoneId);
        }
        public async Task<IEnumerable<RepairZone>> GetAllRepairZonesAsync()
        {
            return await _context.RepairZones.ToListAsync();
        }
        public async Task<IEnumerable<RepairZone>> GetAllRepairZonesByWorkAreaIdAsync(long workAreaId)
        {
            return await _context.RepairZones.Where(x => x.WorkAreaId == workAreaId).ToListAsync();
        }
        public void UpdateRepairZone(RepairZone repairZone)
        {
            _context.RepairZones.Update(repairZone);
        }

        public void DeleteRepairZone(RepairZone repairZone)
        {
            _context.RepairZones.Remove(repairZone);
        }
    }
}
