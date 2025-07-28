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

namespace RoadRepair.Application.Workers.Queries.GetWorker
{
    public class GetWorkerQueryHandler : IRequestHandler<GetWorkerQuery, ErrorOr<Worker>>
    {
        private IWorkerRepository _workerRepository;
        public GetWorkerQueryHandler(IWorkerRepository workerRepository)
        {
            _workerRepository = workerRepository;
        }

        public async Task<ErrorOr<Worker>> Handle(GetWorkerQuery query, CancellationToken cancellationToken)
        {
            var worker = await _workerRepository.GetWorkerByIdAsync(query.WorkerId);

            return worker is null
                ? Error.NotFound(description: "Worker not found")
                : worker;
        }
    }
}
