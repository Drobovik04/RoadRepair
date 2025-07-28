using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.RepairEventWorkers.GetRepairEventWorker
{
    public record GetRepairEventWorkerResponse(long Id, long RepairEventId, long WorkerId);
}
