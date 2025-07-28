using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.RepairEventWorkers.Commands.UpdateRepairEventWorker
{
    public record UpdateRepairEventWorkerCommand(long RepairEventWorkerId, long RepairEventId, long WorkerId) : IRequest<ErrorOr<bool>>;
}
