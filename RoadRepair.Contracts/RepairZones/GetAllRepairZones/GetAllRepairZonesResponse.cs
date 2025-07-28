using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.RepairZones.GetAllRepairZones
{
    public record RepairZoneInfo(long Id, long WorkAreaId, string? GeometryJson, string Name);
    public record GetAllRepairZonesResponse(List<RepairZoneInfo> Values);
}
