using ErrorOr;
using MediatR;
using RoadRepair.Application.Interfaces;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Domain.Entities;

namespace RoadRepair.Application.WorkAreaWorkers.Commands.UpdateWorkAreaWorker
{
    public class UpdateWorkAreaWorkerCommandHandler : IRequestHandler<UpdateWorkAreaWorkerCommand, ErrorOr<bool>>
    {
        private IWorkAreaWorkerRepository _workAreaWorkerRepository;
        private IUnitOfWork _unitOfWork;
        public UpdateWorkAreaWorkerCommandHandler(IWorkAreaWorkerRepository workAreaWorkerRepository, IUnitOfWork unitOfWork)
        {
            _workAreaWorkerRepository = workAreaWorkerRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(UpdateWorkAreaWorkerCommand request, CancellationToken cancellationToken)
        {
            var workAreaWorker = await _workAreaWorkerRepository.GetWorkAreaWorkerByIdAsync(request.WorkAreaWorkerId);

            if (workAreaWorker == null)
            {
                return Error.NotFound(description: "WorkAreaWorker not found");
            }

            workAreaWorker.WorkAreaId = request.WorkAreaId;
            workAreaWorker.WorkerId = request.WorkerId;

            _workAreaWorkerRepository.UpdateWorkAreaWorker(workAreaWorker);

            await _unitOfWork.CommitChangesAsync();

            return true;
        }
    }
}
