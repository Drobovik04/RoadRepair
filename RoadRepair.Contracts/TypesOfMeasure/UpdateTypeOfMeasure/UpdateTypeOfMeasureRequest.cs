using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.TypesOfMeasure.UpdateTypeOfMeasure
{
    public record UpdateTypeOfMeasureRequest(string Name, string ShortName);
}
