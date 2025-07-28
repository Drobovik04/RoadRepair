using ErrorOr;
using MediatR;
using RoadRepair.Application.Interfaces;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Domain.Entities;

namespace RoadRepair.Application.RepairEvents.Commands.UpdateRepairEvent
{
    public class UpdateRepairEventCommandHandler : IRequestHandler<UpdateRepairEventCommand, ErrorOr<bool>>
    {
        private IRepairEventRepository _repairEventRepository;
        private IUnitOfWork _unitOfWork;
        public UpdateRepairEventCommandHandler(IRepairEventRepository repairEventRepository, IUnitOfWork unitOfWork)
        {
            _repairEventRepository = repairEventRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(UpdateRepairEventCommand request, CancellationToken cancellationToken)
        {
            var repairEvent = await _repairEventRepository.GetRepairEventByIdAsync(request.RepairEventId);

            if (repairEvent == null)
            {
                return Error.NotFound(description: "RepairEvent not found");
            }

            repairEvent.RepairZoneId = request.RepairZoneId;
            repairEvent.StartedAt = request.StartedAt;
            repairEvent.EndedAt = request.EndedAt;
            repairEvent.TypeOfRepairId = request.TypeOfRepairId;

            _repairEventRepository.UpdateRepairEvent(repairEvent);

            await _unitOfWork.CommitChangesAsync();

            return true;
        }
    }
}
