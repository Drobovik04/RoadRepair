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

namespace RoadRepair.Application.RepairEventWorkers.Queries.GetRepairEventWorker
{
    public class GetRepairEventWorkerQueryHandler : IRequestHandler<GetRepairEventWorkerQuery, ErrorOr<RepairEventWorker>>
    {
        private IRepairEventWorkerRepository _repairEventWorkersRepository;
        public GetRepairEventWorkerQueryHandler(IRepairEventWorkerRepository repairEventWorkersRepository)
        {
            _repairEventWorkersRepository = repairEventWorkersRepository;
        }

        public async Task<ErrorOr<RepairEventWorker>> Handle(GetRepairEventWorkerQuery query, CancellationToken cancellationToken)
        {
            var repairEventWorker = await _repairEventWorkersRepository.GetRepairEventWorkerByIdAsync(query.RepairEventWorkerId);

            return repairEventWorker is null
                ? Error.NotFound(description: "RepairEventWorker not found")
                : repairEventWorker;
        }
    }
}
