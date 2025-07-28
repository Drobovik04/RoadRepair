using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Interfaces.Repositories
{
    public interface ITypeOfServiceRepository
    {
        Task<IEnumerable<TypeOfService>> GetAllTypesOfServiceAsync();
        Task<TypeOfService?> GetTypeOfServiceByIdAsync(long id);
        Task AddTypeOfServiceAsync(TypeOfService typeOfService);
        void UpdateTypeOfService(TypeOfService typeOfService);
        void DeleteTypeOfService(TypeOfService typeOfService);
    }
}
