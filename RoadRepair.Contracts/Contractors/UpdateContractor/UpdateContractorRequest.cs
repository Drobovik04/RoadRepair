using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.Contractors.UpdateContractor
{
    public record UpdateContractorRequest(string Name, string Address, string? Email, string? ContactPhone, int UNP);
}
