using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Interfaces.Repositories
{
    public interface IContractorRepository
    {
        Task<IEnumerable<Contractor>> GetAllContractorsAsync();
        Task<Contractor?> GetContractorByIdAsync(long id);
        Task AddContractorAsync(Contractor contractor);
        void UpdateContractor(Contractor contractor);
        void DeleteContractor(Contractor contractor);
    }
}
