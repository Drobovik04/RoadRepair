using ErrorOr;
using MediatR;
using RoadRepair.Application.Interfaces;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Domain.Entities;

namespace RoadRepair.Application.RepairEvents.Commands.CreateRepairEvent
{
    public class CreateRepairEventCommandHandler : IRequestHandler<CreateRepairEventCommand, ErrorOr<RepairEvent>>
    {
        private IRepairEventRepository _repairEventRepository;
        private IUnitOfWork _unitOfWork;
        public CreateRepairEventCommandHandler(IRepairEventRepository repairEventRepository, IUnitOfWork unitOfWork)
        {
            _repairEventRepository = repairEventRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<RepairEvent>> Handle(CreateRepairEventCommand request, CancellationToken cancellationToken)
        {
            var repairEvent = new RepairEvent
            {
                RepairZoneId = request.RepairZoneId,
                StartedAt = request.StartedAt,
                EndedAt = request.EndedAt,
                TypeOfRepairId = request.TypeOfRepairId,
            };

            await _repairEventRepository.AddRepairEventAsync(repairEvent);

            await _unitOfWork.CommitChangesAsync();

            return repairEvent;
        }
    }
}
