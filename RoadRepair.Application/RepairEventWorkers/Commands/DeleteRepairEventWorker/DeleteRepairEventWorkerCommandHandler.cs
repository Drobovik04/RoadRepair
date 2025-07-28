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

namespace RoadRepair.Application.RepairEventWorkers.Commands.DeleteRepairEventWorker
{
    public class DeleteRepairEventWorkerCommandHandler : IRequestHandler<DeleteRepairEventWorkerCommand, ErrorOr<bool>>
    {
        private IRepairEventWorkerRepository _repairEventWorkerRepository;
        private IUnitOfWork _unitOfWork;
        public DeleteRepairEventWorkerCommandHandler(IRepairEventWorkerRepository repairEventWorkerRepository, IUnitOfWork unitOfWork)
        {
            _repairEventWorkerRepository = repairEventWorkerRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(DeleteRepairEventWorkerCommand request, CancellationToken cancellationToken)
        {
            var repairEventWorker = await _repairEventWorkerRepository.GetRepairEventWorkerByIdAsync(request.Id);

            if (repairEventWorker == null)
            {
                return Error.NotFound(description: "RepairEventWorker not found");
            }

            _repairEventWorkerRepository.DeleteRepairEventWorker(repairEventWorker);

            await _unitOfWork.CommitChangesAsync();

            return true;
        }
    }
}
