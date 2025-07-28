using ErrorOr;
using MediatR;
using RoadRepair.Domain.Entities;

namespace RoadRepair.Application.RepairZones.Commands.CreateRepairZone
{
    public record CreateRepairZoneCommand(long WorkAreaId, string? GeometryJson, string Name) : IRequest<ErrorOr<RepairZone>>;
}
