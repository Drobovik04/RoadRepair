using ErrorOr;
using MediatR;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.WorkAreaWorkers.Queries.GetAllWorkAreaWorkersByWorkAreaId
{
    public class GetAllWorkAreaWorkersQueryHandler : IRequestHandler<GetAllWorkAreaWorkersByWorkAreaIdQuery, ErrorOr<List<WorkAreaWorker>>>
    {
        private IWorkAreaWorkerRepository _workAreaWorkerRepository;
        public GetAllWorkAreaWorkersQueryHandler(IWorkAreaWorkerRepository workAreaWorkerRepository)
        {
            _workAreaWorkerRepository = workAreaWorkerRepository;
        }

        public async Task<ErrorOr<List<WorkAreaWorker>>> Handle(GetAllWorkAreaWorkersByWorkAreaIdQuery query, CancellationToken cancellationToken)
        {
            var workAreasWorkers = await _workAreaWorkerRepository.GetWorkAreaWorkerByWorkAreaIdAsync(query.WorkAreaId);

            return workAreasWorkers is null
                ? Error.NotFound(description: "WorkAreasWorkers are not found")
                : workAreasWorkers.ToList();
        }
    }
}
