using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.RepairEvents.Commands.DeleteRepairEvent
{
    public record DeleteRepairEventCommand(long Id) : IRequest<ErrorOr<bool>>;
}
