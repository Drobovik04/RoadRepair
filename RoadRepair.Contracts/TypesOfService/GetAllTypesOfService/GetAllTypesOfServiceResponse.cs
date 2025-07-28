using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.TypesOfService.GetAllTypesOfService
{
    public record TypeOfServiceInfo(long Id, string Name);
    public record GetAllTypesOfServiceResponse(List<TypeOfServiceInfo> Values);
}
