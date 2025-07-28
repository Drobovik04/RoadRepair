using ErrorOr;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Application.Interfaces;
using MediatR;

namespace RoadRepair.Application.Workers.Commands.UpdateWorker
{
    public class UpdateWorkerCommandHandler : IRequestHandler<UpdateWorkerCommand, ErrorOr<bool>>
    {
        private IWorkerRepository _workerRepository;
        private IUnitOfWork _unitOfWork;
        public UpdateWorkerCommandHandler(IWorkerRepository workerRepository, IUnitOfWork unitOfWork)
        {
            _workerRepository = workerRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(UpdateWorkerCommand request, CancellationToken cancellationToken)
        {
            var worker = await _workerRepository.GetWorkerByIdAsync(request.WorkerId);

            if (worker == null)
            {
                return Error.NotFound(description: "Worker not found");
            }

            if (worker.HiredAt > request.FiredAt)
            {
                return Error.Failure(description: "FiredAt must be greater than HiredAt");
            }

            worker.PositionId = request.PositionId;
            worker.LastName = request.LastName;
            worker.MiddleName = request.MiddleName;
            worker.FirstName = request.FirstName;
            worker.HiredAt = request.HiredAt;
            worker.FiredAt = request.FiredAt;
            worker.PositionId = request.PositionId;

            _workerRepository.UpdateWorker(worker);

            await _unitOfWork.CommitChangesAsync();

            return true;
        }
    }
}
