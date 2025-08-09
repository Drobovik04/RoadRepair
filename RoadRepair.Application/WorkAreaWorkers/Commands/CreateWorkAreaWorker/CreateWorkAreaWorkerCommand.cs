using ErrorOr;
using MediatR;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.WorkAreaWorkers.Commands.CreateWorkAreaWorker
{
    public record CreateWorkAreaWorkerCommand(long WorkAreaId, long WorkerId) : IRequest<ErrorOr<WorkAreaWorker>>;
}
