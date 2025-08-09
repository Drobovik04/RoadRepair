using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.WorkTimes.BulkUpdateWorkTime
{
    public record WorkTimeInfo(long Id, DateOnly DayOfWork, float Hours, long WorkAreaWorkerId);
    public record BulkUpdateWorkTimeRequest(List<WorkTimeInfo> Values);
}
