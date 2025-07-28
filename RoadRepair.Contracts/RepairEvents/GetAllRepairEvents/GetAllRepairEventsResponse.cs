using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.RepairEvents.GetAllRepairEvents
{
    public record RepairEventInfo(long Id, long RepairZoneId, DateOnly StartedAt, DateOnly? EndedAt, long TypeOfRepairId, string TypeOfRepairName);
    public record GetAllRepairEventsResponse(List<RepairEventInfo> Values);
}
