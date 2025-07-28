using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.RepairEventMedia.Commands.UpdateRepairEventMedia
{
    public record UpdateRepairEventMediaCommand(long RepairEventMediaId, long RepairEventId, IFormFile? File, string? Description) : IRequest<ErrorOr<bool>>;
}
