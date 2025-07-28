using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.TypesOfMeasure.Commands.DeleteTypeOfMeasure
{
    public record DeleteTypeOfMeasureCommand(long Id) : IRequest<ErrorOr<bool>>;
}
