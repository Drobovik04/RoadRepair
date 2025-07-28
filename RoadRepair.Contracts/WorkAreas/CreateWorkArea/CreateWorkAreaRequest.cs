using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.WorkAreas.CreateWorkArea
{
    public record CreateWorkAreaRequest(long OrganizationId, string Name, string? Description, DateOnly? CreatedAt, DateOnly? UpdatedAt, long? ResponsibleId);
}
