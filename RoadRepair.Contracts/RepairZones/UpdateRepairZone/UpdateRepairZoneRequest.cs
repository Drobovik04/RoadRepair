using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.RepairZones.UpdateRepairZone
{
    public record UpdateRepairZoneRequest(long WorkAreaId, string? GeometryJson, string Name);
}
