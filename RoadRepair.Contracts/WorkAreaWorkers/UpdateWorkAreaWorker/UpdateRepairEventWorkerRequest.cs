using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.WorkAreaWorkers.UpdateWorkAreaWorker
{
    public record UpdateWorkAreaWorkerRequest(long WorkAreaId, long WorkerId);
}
