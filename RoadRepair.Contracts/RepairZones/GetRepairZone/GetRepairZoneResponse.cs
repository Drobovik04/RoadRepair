using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.RepairZones.GetRepairZone
{
    public record GetRepairZoneResponse(long Id,long WorkAreaId, string? GeometryJson, string Name);
}
