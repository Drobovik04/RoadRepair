using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Interfaces.Repositories
{
    public interface IRepairZoneRepository
    {
        Task<IEnumerable<RepairZone>> GetAllRepairZonesAsync();
        Task<IEnumerable<RepairZone>> GetAllRepairZonesByWorkAreaIdAsync(long workAreaId);
        Task<RepairZone?> GetRepairZoneByIdAsync(long id);
        Task AddRepairZoneAsync(RepairZone repairZone);
        void UpdateRepairZone(RepairZone repairZone);
        void DeleteRepairZone(RepairZone repairZone);
    }
}
