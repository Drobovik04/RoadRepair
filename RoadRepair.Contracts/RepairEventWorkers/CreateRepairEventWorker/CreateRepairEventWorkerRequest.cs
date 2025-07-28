using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.RepairEventWorkers.CreateRepairEventWorker
{
    public record CreateRepairEventWorkerRequest(long RepairEventId, long WorkerId);
}
