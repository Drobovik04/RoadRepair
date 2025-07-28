using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.Contractors.GetAllContractors
{
    public record ContractorInfo(long Id, string Name, string Address, string? Email, string? ContactPhone, long UNP);
    public record GetAllContractorsResponse(List<ContractorInfo> Values);
}
