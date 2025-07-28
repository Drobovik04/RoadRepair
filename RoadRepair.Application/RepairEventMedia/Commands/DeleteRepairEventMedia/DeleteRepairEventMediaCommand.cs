using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.RepairEventMedia.Commands.DeleteRepairEventMedia
{
    public record DeleteRepairEventMediaCommand(long Id) : IRequest<ErrorOr<bool>>;
}
