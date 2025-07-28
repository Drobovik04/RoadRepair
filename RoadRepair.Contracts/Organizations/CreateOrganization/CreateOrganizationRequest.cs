using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.Organizations.CreateOrganization
{
    public record CreateOrganizationRequest(string Name, string? Address, string? ContactPhone, string? Email, int UNP, long? ParentId);
}
