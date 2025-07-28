using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Interfaces.Repositories
{
    public interface IWorkTimeRepository
    {
        Task<IEnumerable<WorkTime>> GetAllWorkTimesAsync();
        Task<IEnumerable<WorkTime>> GetAllWorkTimesByRepairEventWorkerIdAsync(long repairEventWorkerId);
        Task<WorkTime?> GetWorkTimeByIdAsync(long id);
        Task AddWorkTimeAsync(WorkTime workTime);
        void UpdateWorkTime(WorkTime workTime);
        void DeleteWorkTime(WorkTime workTime);
        void DeleteAllWorkTimes(List<WorkTime> workTimes);
    }
}
