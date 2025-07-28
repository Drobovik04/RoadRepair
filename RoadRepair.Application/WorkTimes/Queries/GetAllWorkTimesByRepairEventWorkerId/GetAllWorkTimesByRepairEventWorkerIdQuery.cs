using ErrorOr;
using MediatR;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.WorkTimes.Queries.GetAllWorkTimesByRepairEventWorkerId
{
    public record GetAllWorkTimesByRepairEventWorkerIdQuery(long RepairEventWorkerId) : IRequest<ErrorOr<List<WorkTime>>>;
}
