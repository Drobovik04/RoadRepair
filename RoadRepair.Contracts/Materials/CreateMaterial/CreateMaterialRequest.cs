using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.Materials.CreateMaterial
{
    public record CreateMaterialRequest(string Name, long TypeOfMeasureId, long? OrganizationId);
}
