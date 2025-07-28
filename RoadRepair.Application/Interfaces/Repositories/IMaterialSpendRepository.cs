using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Interfaces.Repositories
{
    public interface IMaterialSpendRepository
    {
        Task<IEnumerable<MaterialSpend>> GetAllMaterialSpendsAsync();
        Task<IEnumerable<MaterialSpend>> GetAllMaterialSpendsByRepairEventIdAsync(long repairEventId);
        Task<MaterialSpend?> GetMaterialSpendByIdAsync(long id);
        Task AddMaterialSpendAsync(MaterialSpend materialSpend);
        void UpdateMaterialSpend(MaterialSpend materialSpend);
        void DeleteMaterialSpend(MaterialSpend materialSpend);
    }
}
