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

namespace RoadRepair.Application.RepairEvents.Queries.GetAllRepairEvents
{
    public class GetAllRepairEventsQueryHandler : IRequestHandler<GetAllRepairEventsQuery, ErrorOr<List<RepairEvent>>>
    {
        private IRepairEventRepository _repairEventRepository;
        public GetAllRepairEventsQueryHandler(IRepairEventRepository repairEventRepository)
        {
            _repairEventRepository = repairEventRepository;
        }

        public async Task<ErrorOr<List<RepairEvent>>> Handle(GetAllRepairEventsQuery query, CancellationToken cancellationToken)
        {
            var repairEvents = await _repairEventRepository.GetAllRepairEventsByRepairZoneIdAsync(query.repairZoneId);

            return repairEvents is null
                ? Error.NotFound(description: "RepairEvents are not found")
                : repairEvents.ToList();
        }
    }
}
