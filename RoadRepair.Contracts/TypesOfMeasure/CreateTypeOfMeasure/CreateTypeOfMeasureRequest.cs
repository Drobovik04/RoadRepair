using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.TypesOfMeasure.CreateTypeOfMeasure
{
    public record CreateTypeOfMeasureRequest(string Name, string ShortName);
}
