using ErrorOr;
using MediatR;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.TypesOfRepair.Commands.CreateTypeOfRepair
{
    public record CreateTypeOfRepairCommand(string Name) : IRequest<ErrorOr<TypeOfRepair>>;
}
