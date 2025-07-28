using ErrorOr;
using MediatR;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Workers.Commands.CreateWorker
{
    public record CreateWorkerCommand(long OrganizationId, string LastName, string? MiddleName, string FirstName, DateOnly HiredAt, long PositionId) : IRequest<ErrorOr<Worker>>;
}
