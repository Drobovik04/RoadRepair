using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace RoadRepair.Application.RepairEventMedia.Commands.CreateRepairEventMedia
{
    public record CreateRepairEventMediaCommand(long RepairEventId, IFormFile File, string? Description) : IRequest<ErrorOr<Domain.Entities.RepairEventMedia>>;
}
