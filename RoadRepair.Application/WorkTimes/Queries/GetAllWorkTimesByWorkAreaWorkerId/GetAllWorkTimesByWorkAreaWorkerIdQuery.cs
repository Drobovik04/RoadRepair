using ErrorOr;
using MediatR;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.WorkTimes.Queries.GetAllWorkTimesByWorkAreaWorkerId
{
    public record GetAllWorkTimesByWorkAreaWorkerIdQuery(long WorkAreaWorkerId) : IRequest<ErrorOr<List<WorkTime>>>;
}
