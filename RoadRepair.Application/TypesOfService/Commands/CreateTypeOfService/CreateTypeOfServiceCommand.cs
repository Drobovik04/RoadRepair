using ErrorOr;
using MediatR;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.TypesOfService.Commands.CreateTypeOfService
{
    public record CreateTypeOfServiceCommand(string Name) : IRequest<ErrorOr<TypeOfService>>;
}
