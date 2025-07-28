using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.RepairEvents.CreateRepairEvent
{
    public record CreateRepairEventRequest(long RepairZoneId, DateOnly StartedAt, DateOnly? EndedAt, long TypeOfRepairId);
}
