using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.Materials.UpdateMaterial
{
    public record UpdateMaterialRequest(string Name, long TypeOfMeasureId, long? OrganizationId);
}
