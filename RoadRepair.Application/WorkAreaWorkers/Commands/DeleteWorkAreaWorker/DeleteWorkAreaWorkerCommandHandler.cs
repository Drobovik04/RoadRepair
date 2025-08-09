using ErrorOr;
using MediatR;
using RoadRepair.Application.Interfaces;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Application.Organizations.Commands.DeleteOrganization;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.WorkAreaWorkers.Commands.DeleteWorkAreaWorker
{
    public class DeleteWorkAreaWorkerCommandHandler : IRequestHandler<DeleteWorkAreaWorkerCommand, ErrorOr<bool>>
    {
        private IWorkAreaWorkerRepository _workAreaWorkerRepository;
        private IUnitOfWork _unitOfWork;
        public DeleteWorkAreaWorkerCommandHandler(IWorkAreaWorkerRepository workAreaWorkerRepository, IUnitOfWork unitOfWork)
        {
            _workAreaWorkerRepository = workAreaWorkerRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(DeleteWorkAreaWorkerCommand request, CancellationToken cancellationToken)
        {
            var workAreaWorker = await _workAreaWorkerRepository.GetWorkAreaWorkerByIdAsync(request.Id);

            if (workAreaWorker == null)
            {
                return Error.NotFound(description: "WorkAreaWorker not found");
            }

            _workAreaWorkerRepository.DeleteWorkAreaWorker(workAreaWorker);

            await _unitOfWork.CommitChangesAsync();

            return true;
        }
    }
}
