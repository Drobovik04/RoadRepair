using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.TypesOfService.Commands.DeleteTypeOfService
{
    public record DeleteTypeOfServiceCommand(long Id) : IRequest<ErrorOr<bool>>;
}
