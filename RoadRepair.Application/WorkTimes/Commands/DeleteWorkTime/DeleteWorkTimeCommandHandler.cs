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

namespace RoadRepair.Application.WorkTimes.Commands.DeleteWorkTime
{
    public class DeleteWorkTimeCommandHandler : IRequestHandler<DeleteWorkTimeCommand, ErrorOr<bool>>
    {
        private IWorkTimeRepository _workTimeRepository;
        private IUnitOfWork _unitOfWork;
        public DeleteWorkTimeCommandHandler(IWorkTimeRepository workTimeRepository, IUnitOfWork unitOfWork)
        {
            _workTimeRepository = workTimeRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(DeleteWorkTimeCommand request, CancellationToken cancellationToken)
        {
            var workTime = await _workTimeRepository.GetWorkTimeByIdAsync(request.Id);

            if (workTime == null)
            {
                return Error.NotFound(description: "WorkTime not found");
            }

            _workTimeRepository.DeleteWorkTime(workTime);

            await _unitOfWork.CommitChangesAsync();

            return true;
        }
    }
}
