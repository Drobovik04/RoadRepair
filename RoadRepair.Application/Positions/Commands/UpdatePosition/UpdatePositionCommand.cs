using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Positions.Commands.UpdatePosition
{
    public record UpdatePositionCommand(long PositionId, string Name) : IRequest<ErrorOr<bool>>;
}
