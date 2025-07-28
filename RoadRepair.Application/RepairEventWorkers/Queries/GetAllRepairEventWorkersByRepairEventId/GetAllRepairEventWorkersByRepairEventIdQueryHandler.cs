using ErrorOr;
using MediatR;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Application.Organizations.Queries.GetOrganization;
using RoadRepair.Application.RepairEventWorkers.Queries.GetAllRepairEventWorkersByRepairEventId;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.RepairEventWorkers.Queries.GetAllRepairEventWorkersByRepairEventId
{
    public class GetAllRepairEventWorkersQueryHandler : IRequestHandler<GetAllRepairEventWorkersByRepairEventIdQuery, ErrorOr<List<RepairEventWorker>>>
    {
        private IRepairEventWorkerRepository _repairEventWorkerRepository;
        public GetAllRepairEventWorkersQueryHandler(IRepairEventWorkerRepository repairEventWorkerRepository)
        {
            _repairEventWorkerRepository = repairEventWorkerRepository;
        }

        public async Task<ErrorOr<List<RepairEventWorker>>> Handle(GetAllRepairEventWorkersByRepairEventIdQuery query, CancellationToken cancellationToken)
        {
            var repairEventWorkers = await _repairEventWorkerRepository.GetRepairEventWorkerByRepairEventIdAsync(query.RepairEventId);

            return repairEventWorkers is null
                ? Error.NotFound(description: "RepairEventsWorkers are not found")
                : repairEventWorkers.ToList();
        }
    }
}
