using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.Contractors.CreateContractor
{
    public record CreateContractorRequest(string Name, string Address, string? Email, string? ContactPhone, int UNP);
}
