using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.Positions.GetAllPositions
{
    public record PositionInfo(long Id, string Name);
    public record GetAllPositionsResponse(List<PositionInfo> Values);
}
