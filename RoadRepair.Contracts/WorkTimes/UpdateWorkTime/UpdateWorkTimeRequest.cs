using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.WorkTimes.UpdateWorkTime
{
    public record UpdateWorkTimeRequest(DateOnly DayOfWork, double Hours, long RepairEventWorkerId);
}
