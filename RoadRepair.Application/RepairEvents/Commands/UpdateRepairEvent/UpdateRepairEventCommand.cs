using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.RepairEvents.Commands.UpdateRepairEvent
{
    public record UpdateRepairEventCommand(long RepairEventId, long RepairZoneId, DateOnly StartedAt, DateOnly? EndedAt, long TypeOfRepairId) : IRequest<ErrorOr<bool>>;
}
