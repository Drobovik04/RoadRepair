using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.TypesOfMeasure.GetAllTypesOfMeasure
{
    public record TypeOfMaterialInfo(long Id, string Name, string ShortName);
    public record GetAllTypesOfMeasureResponse(List<TypeOfMaterialInfo> Values);
}
