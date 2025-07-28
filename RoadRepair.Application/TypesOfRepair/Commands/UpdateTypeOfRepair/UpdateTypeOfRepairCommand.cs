using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.TypesOfRepair.Commands.UpdateTypeOfRepair
{
    public record UpdateTypeOfRepairCommand(long TypeOfRepairId, string Name) : IRequest<ErrorOr<bool>>;
}
