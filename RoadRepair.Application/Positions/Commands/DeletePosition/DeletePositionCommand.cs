using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Positions.Commands.DeletePosition
{
    public record DeletePositionCommand(long Id) : IRequest<ErrorOr<bool>>;
}
