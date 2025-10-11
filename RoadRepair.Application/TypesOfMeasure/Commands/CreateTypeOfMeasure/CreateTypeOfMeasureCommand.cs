using ErrorOr;
using MediatR;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.TypesOfMeasure.Commands.CreateTypeOfMeasure
{
    public record CreateTypeOfMeasureCommand(string Name, string ShortName) : IRequest<ErrorOr<TypeOfMeasure>>;
}
