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
    public class RepairEventsRepository : IRepairEventRepository
    {
        private readonly AppDbContext _context;
        public RepairEventsRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddRepairEventAsync(RepairEvent repairEvent)
        {
            await _context.RepairEvents.AddAsync(repairEvent);
        }
        public async Task<RepairEvent?> GetRepairEventByIdAsync(long repairEventId)
        {
            return await _context.RepairEvents.Include(x => x.RepairEventMedia).FirstOrDefaultAsync(x => x.Id == repairEventId); // include для отслеживания state, чтобы удалить файлы на диске через interceptor, в бд все и так работает
        }

        public async Task<IEnumerable<RepairEvent>> GetAllRepairEventsByRepairZoneIdAsync(long repairZoneId)
        {
            return await _context.RepairEvents.Include(x => x.TypeOfRepair).Where(x => x.RepairZoneId == repairZoneId).ToListAsync();
        }
        public async Task<IEnumerable<RepairEvent>> GetAllRepairEventsAsync()
        {
            return await _context.RepairEvents.ToListAsync();
        }
        public void UpdateRepairEvent(RepairEvent repairEvent)
        {
            _context.RepairEvents.Update(repairEvent);
        }
        public void DeleteRepairEvent(RepairEvent repairEvent)
        {
            _context.RepairEvents.Remove(repairEvent);
        }
    }
}
