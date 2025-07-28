using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.ContractorServices.CreateContractorService
{
    public record CreateContractorServiceRequest(long TypeOfServiceId, long ContractorId, decimal Price, string? Description, DateOnly DateOfService, long WorkAreaId);
}
