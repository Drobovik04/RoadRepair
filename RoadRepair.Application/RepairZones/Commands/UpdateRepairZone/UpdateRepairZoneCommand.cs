using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.RepairZones.Commands.UpdateRepairZone
{
    public record UpdateRepairZoneCommand(long RepairZoneId, long WorkAreaId, string? GeometryJson, string Name) : IRequest<ErrorOr<bool>>;
}
