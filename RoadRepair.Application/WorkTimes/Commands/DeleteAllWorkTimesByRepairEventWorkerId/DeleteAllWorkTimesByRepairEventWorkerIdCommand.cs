using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.WorkTimes.Commands.DeleteAllWorkTimesByRepairEventWorkerId
{
    public record DeleteAllWorkTimesByRepairEventWorkerIdCommand(long RepairEventWorkerId) : IRequest<ErrorOr<bool>>;
}
