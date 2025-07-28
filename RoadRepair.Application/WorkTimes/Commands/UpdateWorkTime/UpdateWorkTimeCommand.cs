using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.WorkTimes.Commands.UpdateWorkTime
{
    public record UpdateWorkTimeCommand(long WorkTimeId, DateOnly DayOfWork, double Hours, long RepairEventWorkerId) : IRequest<ErrorOr<bool>>;
}
