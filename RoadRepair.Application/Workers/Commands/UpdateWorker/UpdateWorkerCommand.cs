using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Workers.Commands.UpdateWorker
{
    public record UpdateWorkerCommand(long WorkerId, long OrganizationId, string LastName, string? MiddleName, string FirstName, DateOnly HiredAt, DateOnly? FiredAt, long PositionId) : IRequest<ErrorOr<bool>>;
}
