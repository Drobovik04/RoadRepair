using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.TypesOfRepair.Commands.DeleteTypeOfRepair
{
    public record DeleteTypeOfRepairCommand(long Id) : IRequest<ErrorOr<bool>>;
}
