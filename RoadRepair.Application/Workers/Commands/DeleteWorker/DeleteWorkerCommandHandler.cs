using ErrorOr;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Application.Interfaces;
using RoadRepair.Application.Organizations.Commands.DeleteOrganization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using RoadRepair.Application.TypesOfService.Commands.DeleteTypeOfService;

namespace RoadRepair.Application.Workers.Commands.DeleteWorker
{
    public class DeleteWorkerCommandHandler : IRequestHandler<DeleteWorkerCommand, ErrorOr<bool>>
    {
        private IWorkerRepository _workerRepository;
        private IUnitOfWork _unitOfWork;
        public DeleteWorkerCommandHandler(IWorkerRepository workerRepository, IUnitOfWork unitOfWork)
        {
            _workerRepository = workerRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(DeleteWorkerCommand request, CancellationToken cancellationToken)
        {
            var worker = await _workerRepository.GetWorkerByIdAsync(request.Id);

            if (worker == null)
            {
                return Error.NotFound(description: "Worker not found");
            }

            _workerRepository.DeleteWorker(worker);

            await _unitOfWork.CommitChangesAsync();

            return true;
        }
    }
}
