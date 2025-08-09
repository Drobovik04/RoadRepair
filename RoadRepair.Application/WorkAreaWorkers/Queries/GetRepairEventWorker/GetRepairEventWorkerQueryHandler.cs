using ErrorOr;
using MediatR;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Application.Organizations.Queries.GetOrganization;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.WorkAreaWorkers.Queries.GetWorkAreaWorker
{
    public class GetWorkAreaWorkerQueryHandler : IRequestHandler<GetWorkAreaWorkerQuery, ErrorOr<WorkAreaWorker>>
    {
        private IWorkAreaWorkerRepository _workAreaWorkersRepository;
        public GetWorkAreaWorkerQueryHandler(IWorkAreaWorkerRepository workAreaWorkersRepository)
        {
            _workAreaWorkersRepository = workAreaWorkersRepository;
        }

        public async Task<ErrorOr<WorkAreaWorker>> Handle(GetWorkAreaWorkerQuery query, CancellationToken cancellationToken)
        {
            var workAreaWorker = await _workAreaWorkersRepository.GetWorkAreaWorkerByIdAsync(query.WorkAreaWorkerId);

            return workAreaWorker is null
                ? Error.NotFound(description: "WorkAreaWorker not found")
                : workAreaWorker;
        }
    }
}
