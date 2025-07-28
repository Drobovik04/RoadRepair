using ErrorOr;
using MediatR;
using RoadRepair.Application.Interfaces;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Domain.Entities;

namespace RoadRepair.Application.RepairEventWorkers.Commands.CreateRepairEventWorker
{
    public class CreateRepairEventWorkerCommandHandler : IRequestHandler<CreateRepairEventWorkerCommand, ErrorOr<RepairEventWorker>>
    {
        private IRepairEventWorkerRepository _repairEventWorkerRepository;
        private IUnitOfWork _unitOfWork;
        public CreateRepairEventWorkerCommandHandler(IRepairEventWorkerRepository repairEventWorkerRepository, IUnitOfWork unitOfWork)
        {
            _repairEventWorkerRepository = repairEventWorkerRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<RepairEventWorker>> Handle(CreateRepairEventWorkerCommand request, CancellationToken cancellationToken)
        {
            var repairEventWorker = new RepairEventWorker
            {
                RepairEventId = request.RepairEventId,
                WorkerId = request.WorkerId,
            };

            await _repairEventWorkerRepository.AddRepairEventWorkerAsync(repairEventWorker);

            await _unitOfWork.CommitChangesAsync();

            return repairEventWorker;
        }
    }
}
