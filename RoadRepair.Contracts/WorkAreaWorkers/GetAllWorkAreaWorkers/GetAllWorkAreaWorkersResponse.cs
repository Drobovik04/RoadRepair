using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.WorkAreaWorkers.GetAllWorkAreaWorkers
{
    public record WorkAreaWorkerInfo(long Id, long WorkAreaWorkerId, long WorkerId);
    public record GetAllWorkAreaWorkersResponse(List<WorkAreaWorkerInfo> Values);
}
