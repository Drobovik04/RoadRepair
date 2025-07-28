using ErrorOr;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Application.Interfaces;
using MediatR;
using RoadRepair.Application.TypesOfMeasure.Commands.UpdateTypeOfMeasure;
using RoadRepair.Domain.Entities;

namespace RoadRepair.Application.RepairZones.Commands.UpdateRepairZone
{
    public class UpdateRepairZoneCommandHandler : IRequestHandler<UpdateRepairZoneCommand, ErrorOr<bool>>
    {
        private IRepairZoneRepository _repairZoneRepository;
        private IUnitOfWork _unitOfWork;
        public UpdateRepairZoneCommandHandler(IRepairZoneRepository repairZoneRepository, IUnitOfWork unitOfWork)
        {
            _repairZoneRepository = repairZoneRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(UpdateRepairZoneCommand request, CancellationToken cancellationToken)
        {
            var repairZone = await _repairZoneRepository.GetRepairZoneByIdAsync(request.RepairZoneId);

            if (repairZone == null)
            {
                return Error.NotFound(description: "RepairZone not found");
            }

            repairZone.WorkAreaId = request.WorkAreaId;
            repairZone.GeometryJson = request.GeometryJson;
            repairZone.Name = request.Name;

            _repairZoneRepository.UpdateRepairZone(repairZone);

            await _unitOfWork.CommitChangesAsync();

            return true;
        }
    }
}
