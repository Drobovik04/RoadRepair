using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.WorkAreaWorkers.GetWorkAreaWorker
{
    public record GetWorkAreaWorkerResponse(long Id, long WorkAreaId, long WorkerId);
}
