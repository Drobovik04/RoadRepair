using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Interfaces.Repositories
{
    public interface IRepairEventRepository
    {
        Task<IEnumerable<RepairEvent>> GetAllRepairEventsAsync();
        Task<IEnumerable<RepairEvent>> GetAllRepairEventsByRepairZoneIdAsync(long repairZoneId);
        Task<RepairEvent?> GetRepairEventByIdAsync(long id);
        Task AddRepairEventAsync(RepairEvent repairEvent);
        void UpdateRepairEvent(RepairEvent repairEvent);
        void DeleteRepairEvent(RepairEvent repairEvent);
    }
}
