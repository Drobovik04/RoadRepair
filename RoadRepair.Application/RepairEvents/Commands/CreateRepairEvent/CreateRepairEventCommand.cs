using ErrorOr;
using MediatR;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.RepairEvents.Commands.CreateRepairEvent
{
    public record CreateRepairEventCommand(long RepairZoneId, DateOnly StartedAt, DateOnly? EndedAt, long TypeOfRepairId) : IRequest<ErrorOr<RepairEvent>>;
}
