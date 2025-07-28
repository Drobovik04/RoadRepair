using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.ContractorServices.GetContractorService
{
    public record GetContractorServiceResponse(long Id, long TypeOfServiceId, long ContractorId, decimal Price, string? Description, DateOnly DateOfService, long WorkAreaId);
}
