using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.RepairEventWorkers.Commands.DeleteRepairEventWorker
{
    public record DeleteRepairEventWorkerCommand(long Id) : IRequest<ErrorOr<bool>>;
}
