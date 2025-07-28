using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.WorkTimes.CreateWorkTime
{
    public record CreateWorkTimeRequest(DateOnly DayOfWork, double Hours, long RepairEventWorkerId);
}
