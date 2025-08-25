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

namespace RoadRepair.Application.RepairEvents.Queries.GetAllRepairEventsByWorkAreaId
{
    public class GetAllRepairEventsByWorkAreaIdQueryHandler : IRequestHandler<GetAllRepairEventsByWorkAreaIdQuery, ErrorOr<List<RepairEvent>>>
    {
        private IRepairEventRepository _repairEventRepository;
        public GetAllRepairEventsByWorkAreaIdQueryHandler(IRepairEventRepository repairEventRepository)
        {
            _repairEventRepository = repairEventRepository;
        }

        public async Task<ErrorOr<List<RepairEvent>>> Handle(GetAllRepairEventsByWorkAreaIdQuery query, CancellationToken cancellationToken)
        {
            var repairEvents = await _repairEventRepository.GetAllRepairEventsByWorkAreaIdAsync(query.workAreaId);

            return repairEvents is null
                ? Error.NotFound(description: "RepairEvents are not found")
                : repairEvents.ToList();
        }
    }
}
