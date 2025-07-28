using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Materials.Commands.DeleteMaterial
{
    public record DeleteMaterialCommand(long Id) : IRequest<ErrorOr<bool>>;
}
