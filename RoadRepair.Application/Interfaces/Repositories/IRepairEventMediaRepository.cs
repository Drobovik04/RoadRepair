using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Interfaces.Repositories
{
    public interface IRepairEventMediaRepository
    {
        Task<IEnumerable<Domain.Entities.RepairEventMedia>> GetAllRepairEventMediaAsync();

        Task<IEnumerable<Domain.Entities.RepairEventMedia>> GetAllRepairEventMediaByRepairZoneIdAsync(long repairEventId);
        Task<Domain.Entities.RepairEventMedia?> GetRepairEventMediaByIdAsync(long id);
        Task AddRepairEventMediaAsync(Domain.Entities.RepairEventMedia repairEventMedia);
        void UpdateRepairEventMedia(Domain.Entities.RepairEventMedia repairEventMedia);
        void DeleteRepairEventMedia(Domain.Entities.RepairEventMedia repairEventMedia);
    }
}
