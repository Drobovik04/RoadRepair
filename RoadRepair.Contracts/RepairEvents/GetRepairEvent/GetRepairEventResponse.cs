using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.RepairEvents.GetRepairEvent
{
    public record GetRepairEventResponse(long Id, long RepairZoneId, DateOnly StartedAt, DateOnly? EndedAt, long TypeOfRepairId);
}
