using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.Contractors.GetContractor
{
    public record GetContractorResponse(long Id, string Name, string Address, string? Email, string? ContactPhone, int UNP);
}
