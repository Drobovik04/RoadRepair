using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.WorkTimes.Commands.DeleteAllWorkTimesByWorkAreaWorkerId
{
    public record DeleteAllWorkTimesByWorkAreaWorkerIdCommand(long WorkAreaWorkerId) : IRequest<ErrorOr<bool>>;
}
