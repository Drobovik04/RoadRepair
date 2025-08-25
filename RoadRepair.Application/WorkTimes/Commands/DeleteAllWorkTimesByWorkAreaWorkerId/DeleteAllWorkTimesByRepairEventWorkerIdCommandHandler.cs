using ErrorOr;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using RoadRepair.Domain.Entities;

namespace RoadRepair.Application.WorkTimes.Commands.DeleteAllWorkTimesByWorkAreaWorkerId
{
    public class DeleteAllWorkTimesByWorkAreaWorkerIdCommandHandler : IRequestHandler<DeleteAllWorkTimesByWorkAreaWorkerIdCommand, ErrorOr<bool>>
    {
        private IWorkTimeRepository _workTimeRepository;
        private IUnitOfWork _unitOfWork;
        public DeleteAllWorkTimesByWorkAreaWorkerIdCommandHandler(IWorkTimeRepository workTimeRepository, IUnitOfWork unitOfWork)
        {
            _workTimeRepository = workTimeRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(DeleteAllWorkTimesByWorkAreaWorkerIdCommand request, CancellationToken cancellationToken)
        {
            var workTimes = await _workTimeRepository.GetAllWorkTimesByWorkAreaWorkerIdAsync(request.WorkAreaWorkerId);

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
