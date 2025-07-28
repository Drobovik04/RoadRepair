using ErrorOr;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Application.Interfaces;
using RoadRepair.Application.Organizations.Commands.CreateOrganization;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace RoadRepair.Application.RepairZones.Commands.CreateRepairZone
{
    public class CreateRepairZoneCommandHandler : IRequestHandler<CreateRepairZoneCommand, ErrorOr<RepairZone>>
    {
        private IRepairZoneRepository _repairZoneRepository;
        private IUnitOfWork _unitOfWork;
        public CreateRepairZoneCommandHandler(IRepairZoneRepository repairZoneRepository, IUnitOfWork unitOfWork)
        {
            _repairZoneRepository = repairZoneRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<RepairZone>> Handle(CreateRepairZoneCommand request, CancellationToken cancellationToken)
        {
            var repairZone = new RepairZone
            {
                WorkAreaId = request.WorkAreaId,
                GeometryJson = request.GeometryJson,
                Name = request.Name,
            };

            await _repairZoneRepository.AddRepairZoneAsync(repairZone);

            await _unitOfWork.CommitChangesAsync();

            return repairZone;
        }
    }
}
