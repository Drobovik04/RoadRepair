using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.WorkTimes.GetAllWorkTimesByRepairEventWorkerId
{
    public record WorkTimeInfo(long Id, DateOnly DayOfWork, float Hours, long RepairEventWorkerId);
    public record GetAllWorkAreasResponse(List<WorkTimeInfo> Values);
}
