using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.ContractorServices.UpdateContractorService
{
    public record UpdateContractorServiceRequest(long TypeOfServiceId, long ContractorId, decimal Price, string? Description, DateOnly DateOfService, long WorkAreaId);
}
