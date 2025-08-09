using ErrorOr;
using MediatR;
using RoadRepair.Application.Interfaces;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Domain.Entities;

namespace RoadRepair.Application.WorkAreaWorkers.Commands.CreateWorkAreaWorker
{
    public class CreateWorkAreaWorkerCommandHandler : IRequestHandler<CreateWorkAreaWorkerCommand, ErrorOr<WorkAreaWorker>>
    {
        private IWorkAreaWorkerRepository _workAreaWorkerRepository;
        private IUnitOfWork _unitOfWork;
        public CreateWorkAreaWorkerCommandHandler(IWorkAreaWorkerRepository workAreaWorkerRepository, IUnitOfWork unitOfWork)
        {
            _workAreaWorkerRepository = workAreaWorkerRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<WorkAreaWorker>> Handle(CreateWorkAreaWorkerCommand request, CancellationToken cancellationToken)
        {
            var workAreaWorker = new WorkAreaWorker
            {
                WorkAreaId = request.WorkAreaId,
                WorkerId = request.WorkerId,
            };

            await _workAreaWorkerRepository.AddWorkAreaWorkerAsync(workAreaWorker);

            await _unitOfWork.CommitChangesAsync();

            return workAreaWorker;
        }
    }
}
