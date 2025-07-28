using ErrorOr;
using MediatR;
using RoadRepair.Application.Interfaces;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Domain.Entities;

namespace RoadRepair.Application.RepairZones.Commands.DeleteRepairZone
{
    public class DeleteRepairZoneCommandHandler : IRequestHandler<DeleteRepairZoneCommand, ErrorOr<bool>>
    {
        private IRepairZoneRepository _repairZoneRepository;
        private IUnitOfWork _unitOfWork;
        public DeleteRepairZoneCommandHandler(IRepairZoneRepository repairZoneRepository, IUnitOfWork unitOfWork)
        {
            _repairZoneRepository = repairZoneRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(DeleteRepairZoneCommand request, CancellationToken cancellationToken)
        {
            var repairZone = await _repairZoneRepository.GetRepairZoneByIdAsync(request.Id);

            if (repairZone == null)
            {
                return Error.NotFound(description: "RepairZone not found");
            }

            _repairZoneRepository.DeleteRepairZone(repairZone);

            await _unitOfWork.CommitChangesAsync();

            return true;
        }
    }
}
