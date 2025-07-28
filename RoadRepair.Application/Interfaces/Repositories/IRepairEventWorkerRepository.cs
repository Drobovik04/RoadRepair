using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Interfaces.Repositories
{
    public interface IRepairEventWorkerRepository
    {
        Task<IEnumerable<RepairEventWorker>> GetAllRepairEventWorkersAsync();
        Task<IEnumerable<RepairEventWorker>> GetRepairEventWorkerByRepairEventIdAsync(long repairEventId);
        Task<RepairEventWorker?> GetRepairEventWorkerByIdAsync(long id);
        Task AddRepairEventWorkerAsync(RepairEventWorker repairEventWorker);
        void UpdateRepairEventWorker(RepairEventWorker repairEventWorker);
        void DeleteRepairEventWorker(RepairEventWorker repairEventWorker);
    }
}
