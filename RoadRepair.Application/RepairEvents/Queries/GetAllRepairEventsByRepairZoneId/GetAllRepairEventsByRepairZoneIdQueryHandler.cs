using ErrorOr;
using MediatR;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.RepairEvents.Queries.GetAllRepairEventsByRepairZoneId
{
    public class GetAllRepairEventsByRepairZoneIdQueryHandler : IRequestHandler<GetAllRepairEventsByRepairZoneIdQuery, ErrorOr<List<RepairEvent>>>
    {
        private IRepairEventRepository _repairEventRepository;
        public GetAllRepairEventsByRepairZoneIdQueryHandler(IRepairEventRepository repairEventRepository)
        {
            _repairEventRepository = repairEventRepository;
        }

        public async Task<ErrorOr<List<RepairEvent>>> Handle(GetAllRepairEventsByRepairZoneIdQuery query, CancellationToken cancellationToken)
        {
            var repairEvents = await _repairEventRepository.GetAllRepairEventsByRepairZoneIdAsync(query.repairZoneId);

            return repairEvents is null
                ? Error.NotFound(description: "RepairEvents are not found")
                : repairEvents.ToList();
        }
    }
}
