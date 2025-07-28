using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.Materials.GetMaterial
{
    public record GetMaterialResponse(long Id, string Name, long TypeOfMeasureId, long? OrganizationId);
}
