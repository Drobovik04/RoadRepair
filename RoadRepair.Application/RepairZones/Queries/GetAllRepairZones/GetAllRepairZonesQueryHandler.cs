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

namespace RoadRepair.Application.RepairZones.Queries.GetAllRepairZones
{
    public class GetAllRepairZonesQueryHandler : IRequestHandler<GetAllRepairZonesQuery, ErrorOr<List<RepairZone>>>
    {
        private IRepairZoneRepository _repairZoneRepository;
        public GetAllRepairZonesQueryHandler(IRepairZoneRepository repairZoneRepository)
        {
            _repairZoneRepository = repairZoneRepository;
        }

        public async Task<ErrorOr<List<RepairZone>>> Handle(GetAllRepairZonesQuery query, CancellationToken cancellationToken)
        {
            var repairZones = await _repairZoneRepository.GetAllRepairZonesByWorkAreaIdAsync(query.WorkAreaId);

            return repairZones is null
                ? Error.NotFound(description: "RepairZones are not found")
                : repairZones.ToList();
        }
    }
}
