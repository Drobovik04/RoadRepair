using ErrorOr;
using MediatR;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Positions.Commands.CreatePosition
{
    public record CreatePositionCommand(string Name) : IRequest<ErrorOr<Position>>;
}
