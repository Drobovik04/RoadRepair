using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Interfaces.Repositories
{
    public interface IMaterialRepository
    {
        Task<IEnumerable<Material>> GetAllMaterialsAsync(long organizationId);
        Task<Material?> GetMaterialByIdAsync(long id);
        Task AddMaterialAsync(Material material);
        void UpdateMaterial(Material material);
        void DeleteMaterial(Material material);
    }
}
