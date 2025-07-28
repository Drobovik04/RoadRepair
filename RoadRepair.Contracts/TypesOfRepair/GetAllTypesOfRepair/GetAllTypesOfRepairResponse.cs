using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.TypesOfRepair.GetAllTypesOfRepair
{
    public record TypeOfRepairInfo(long Id, string Name);
    public record GetAllTypesOfRepairResponse(List<TypeOfRepairInfo> Values);
}
