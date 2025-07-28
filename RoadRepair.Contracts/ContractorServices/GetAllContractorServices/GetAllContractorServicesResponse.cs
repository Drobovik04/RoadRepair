using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.ContractorServices.GetAllContractorServices
{
    public record ContractorServiceInfo(long Id, long TypeOfServiceId, string TypeOfServiceName, long ContractorId, string ContractorName, decimal Price, string? Description, DateOnly DateOfService, long WorkAreaId);
    public record GetAllContractorServicesResponse(List<ContractorServiceInfo> Values);
}
