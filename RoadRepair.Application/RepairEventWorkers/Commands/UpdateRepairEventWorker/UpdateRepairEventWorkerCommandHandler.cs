using ErrorOr;
using MediatR;
using RoadRepair.Application.Interfaces;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Domain.Entities;

namespace RoadRepair.Application.RepairEventWorkers.Commands.UpdateRepairEventWorker
{
    public class UpdateRepairEventWorkerCommandHandler : IRequestHandler<UpdateRepairEventWorkerCommand, ErrorOr<bool>>
    {
        private IRepairEventWorkerRepository _repairEventWorkerRepository;
        private IUnitOfWork _unitOfWork;
        public UpdateRepairEventWorkerCommandHandler(IRepairEventWorkerRepository repairEventWorkerRepository, IUnitOfWork unitOfWork)
        {
            _repairEventWorkerRepository = repairEventWorkerRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(UpdateRepairEventWorkerCommand request, CancellationToken cancellationToken)
        {
            var repairEventWorker = await _repairEventWorkerRepository.GetRepairEventWorkerByIdAsync(request.RepairEventWorkerId);

            if (repairEventWorker == null)
            {
                return Error.NotFound(description: "RepairEventWorker not found");
            }

            repairEventWorker.RepairEventId = request.RepairEventId;
            repairEventWorker.WorkerId = request.WorkerId;

            _repairEventWorkerRepository.UpdateRepairEventWorker(repairEventWorker);

            await _unitOfWork.CommitChangesAsync();

            return true;
        }
    }
}
