using ErrorOr;
using MediatR;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.WorkAreas.Commands.CreateWorkArea
{
    public record CreateWorkAreaCommand(long OrganizationId, string Name, string? Description, DateOnly? CreatedAt,  DateOnly? UpdatedAt, long? ResponsibleId) : IRequest<ErrorOr<WorkArea>>;
}
