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
using RoadRepair.Domain.Entities;

namespace RoadRepair.Application.WorkTimes.Commands.DeleteAllWorkTimesByRepairEventWorkerId
{
    public class DeleteAllWorkTimesByRepairEventWorkerIdCommandHandler : IRequestHandler<DeleteAllWorkTimesByRepairEventWorkerIdCommand, ErrorOr<bool>>
    {
        private IWorkTimeRepository _workTimeRepository;
        private IUnitOfWork _unitOfWork;
        public DeleteAllWorkTimesByRepairEventWorkerIdCommandHandler(IWorkTimeRepository workTimeRepository, IUnitOfWork unitOfWork)
        {
            _workTimeRepository = workTimeRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(DeleteAllWorkTimesByRepairEventWorkerIdCommand request, CancellationToken cancellationToken)
        {
            var workTimes = await _workTimeRepository.GetAllWorkTimesByRepairEventWorkerIdAsync(request.RepairEventWorkerId);

            if (workTimes == null)
            {
                return Error.NotFound(description: "WorkTimes are not found");
            }

            _workTimeRepository.DeleteAllWorkTimes(workTimes.ToList());

            await _unitOfWork.CommitChangesAsync();

            return true;
        }
    }
}
