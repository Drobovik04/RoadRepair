using ErrorOr;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Application.Interfaces;
using RoadRepair.Domain.Entities;
using MediatR;

namespace RoadRepair.Application.Workers.Commands.CreateWorker
{
    public class CreateWorkerCommandHandler : IRequestHandler<CreateWorkerCommand, ErrorOr<Worker>>
    {
        private IWorkerRepository _workerRepository;
        private IUnitOfWork _unitOfWork;
        public CreateWorkerCommandHandler(IWorkerRepository workerRepository, IUnitOfWork unitOfWork)
        {
            _workerRepository = workerRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<Worker>> Handle(CreateWorkerCommand request, CancellationToken cancellationToken)
        {
            var worker = new Worker
            {
                LastName = request.LastName,
                MiddleName = request.MiddleName,
                FirstName = request.FirstName,
                HiredAt = request.HiredAt,
                PositionId = request.PositionId,
            };

            await _workerRepository.AddWorkerAsync(worker);

            await _unitOfWork.CommitChangesAsync();

            return worker;
        }
    }
}
