using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Interfaces.Repositories
{
    public interface IContractorServiceRepository
    {
        Task<IEnumerable<ContractorService>> GetAllContractorServicesAsync();
        Task<IEnumerable<ContractorService>> GetAllContractorServicesByWorkAreaIdAsync(long workAreaId);
        Task<ContractorService?> GetContractorServiceByIdAsync(long id);
        Task AddContractorServiceAsync(ContractorService contractorService);
        void UpdateContractorService(ContractorService contractorService);
        void DeleteContractorService(ContractorService contractorService);
    }
}
