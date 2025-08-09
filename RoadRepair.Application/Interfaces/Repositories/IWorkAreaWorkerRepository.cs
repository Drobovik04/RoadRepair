using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Interfaces.Repositories
{
    public interface IWorkAreaWorkerRepository
    {
        Task<IEnumerable<WorkAreaWorker>> GetAllWorkAreaWorkersAsync();
        Task<IEnumerable<WorkAreaWorker>> GetWorkAreaWorkerByWorkAreaIdAsync(long workAreaId);
        Task<WorkAreaWorker?> GetWorkAreaWorkerByIdAsync(long id);
        Task AddWorkAreaWorkerAsync(WorkAreaWorker workAreaWorker);
        void UpdateWorkAreaWorker(WorkAreaWorker workAreaWorker);
        void DeleteWorkAreaWorker(WorkAreaWorker workAreaWorker);
    }
}
