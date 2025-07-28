using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Materials.Commands.UpdateMaterial
{
    public record UpdateMaterialCommand(long MaterialId, string Name, long TypeOfMeasureId, long? OrganizationId) : IRequest<ErrorOr<bool>>;
}
