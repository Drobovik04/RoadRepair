using ErrorOr;
using MediatR;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Workers.Queries.GetAllWorkers
{
    public class GetAllWorkersQueryHandler : IRequestHandler<GetAllWorkersQuery, ErrorOr<List<Worker>>>
    {
        private IWorkerRepository _workerRepository;
        public GetAllWorkersQueryHandler(IWorkerRepository workerRepository)
        {
            _workerRepository = workerRepository;
        }

        public async Task<ErrorOr<List<Worker>>> Handle(GetAllWorkersQuery query, CancellationToken cancellationToken)
        {
            var workers = await _workerRepository.GetAllWorkersAsync();

            return workers is null
                ? Error.NotFound(description: "Contractors not found")
                : workers.ToList();
        }
    }
}
