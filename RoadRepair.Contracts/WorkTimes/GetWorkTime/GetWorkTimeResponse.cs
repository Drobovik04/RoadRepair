using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.WorkTimes.GetWorkTime
{
    public record GetWorkTimeResponse(long Id, DateOnly DayOfWork, double Hours);
}
