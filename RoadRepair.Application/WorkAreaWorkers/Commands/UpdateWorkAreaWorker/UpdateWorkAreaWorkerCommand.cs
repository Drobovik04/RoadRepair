using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.WorkAreaWorkers.Commands.UpdateWorkAreaWorker
{
    public record UpdateWorkAreaWorkerCommand(long WorkAreaWorkerId, long WorkAreaId, long WorkerId) : IRequest<ErrorOr<bool>>;
}
