using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.RepairZones.Commands.DeleteRepairZone
{
    public record DeleteRepairZoneCommand(long Id) : IRequest<ErrorOr<bool>>;
}
