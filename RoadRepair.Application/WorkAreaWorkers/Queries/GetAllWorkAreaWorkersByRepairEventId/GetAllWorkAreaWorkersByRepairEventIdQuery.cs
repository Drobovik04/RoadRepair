using ErrorOr;
using MediatR;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.WorkAreaWorkers.Queries.GetAllWorkAreaWorkersByWorkAreaId;

public record GetAllWorkAreaWorkersByWorkAreaIdQuery(long WorkAreaId) : IRequest<ErrorOr<List<WorkAreaWorker>>>;
