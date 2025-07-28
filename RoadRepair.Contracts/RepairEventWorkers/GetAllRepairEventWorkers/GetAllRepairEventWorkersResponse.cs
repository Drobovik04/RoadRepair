using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.RepairEventWorkers.GetAllRepairEventWorkers
{
    public record RepairEventWorkerInfo(long Id, long RepairEventWorkerId, long WorkerId);
    public record GetAllRepairEventWorkersResponse(List<RepairEventWorkerInfo> Values);
}
