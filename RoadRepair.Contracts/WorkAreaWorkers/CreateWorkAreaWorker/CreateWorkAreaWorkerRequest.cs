using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.WorkAreaWorkers.CreateWorkAreaWorker
{
    public record CreateWorkAreaWorkerRequest(long WorkAreaId, long WorkerId);
}
