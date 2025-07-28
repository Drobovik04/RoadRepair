using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Interfaces.Repositories
{
    public interface ITypeOfRepairRepository
    {
        Task<IEnumerable<TypeOfRepair>> GetAllTypesOfRepairAsync();
        Task<TypeOfRepair?> GetTypeOfRepairByIdAsync(long id);
        Task AddTypeOfRepairAsync(TypeOfRepair typeOfRepair);
        void UpdateTypeOfRepair(TypeOfRepair typeOfRepair);
        void DeleteTypeOfRepair(TypeOfRepair typeOfRepair);
    }
}
