using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.Materials.GetAllMaterials
{
    public record MaterialInfo(long Id, string Name, long TypeOfMeasureId, string TypeOfMeasureName, string TypeOfMeasureShortName);
    public record GetAllMaterialsResponse(List<MaterialInfo> Values);
}
