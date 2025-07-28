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

namespace RoadRepair.Application.RepairZones.Queries.GetRepairZone
{
    public class GetRepairZoneQueryHandler : IRequestHandler<GetRepairZoneQuery, ErrorOr<RepairZone>>
    {
        private IRepairZoneRepository _repairZoneRepository;
        public GetRepairZoneQueryHandler(IRepairZoneRepository repairZoneRepository)
        {
            _repairZoneRepository = repairZoneRepository;
        }

        public async Task<ErrorOr<RepairZone>> Handle(GetRepairZoneQuery query, CancellationToken cancellationToken)
        {
            var repairZone = await _repairZoneRepository.GetRepairZoneByIdAsync(query.RepairZoneId);

            return repairZone is null
                ? Error.NotFound(description: "RepairZone not found")
                : repairZone;
        }
    }
}
