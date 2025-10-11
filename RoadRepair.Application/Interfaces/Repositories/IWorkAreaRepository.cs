using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Interfaces.Repositories
{
    public interface IWorkAreaRepository
    {
        Task<IEnumerable<WorkArea>> GetAllWorkAreasAsync();
        Task<WorkArea?> GetWorkAreaByIdAsync(long id);
        Task<WorkArea> GetWorkAreaWithAllDependencies(long workAreaId);
        Task AddWorkAreaAsync(WorkArea workArea);
        void UpdateWorkArea(WorkArea workArea);
        void DeleteWorkArea(WorkArea workArea);
    }
}
