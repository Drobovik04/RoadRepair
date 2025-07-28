using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.WorkAreas.Commands.UpdateWorkArea
{
    public record UpdateWorkAreaCommand(long WorkAreaId, long OrganizationId, string Name, string? Description, DateOnly? CreatedAt, DateOnly? UpdatedAt, long? ResponsibleId) : IRequest<ErrorOr<bool>>;
}
