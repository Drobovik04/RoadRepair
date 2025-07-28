using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.WorkAreas.Commands.DeleteWorkArea
{
    public record DeleteWorkAreaCommand(long Id) : IRequest<ErrorOr<bool>>;
}
