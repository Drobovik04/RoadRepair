using ErrorOr;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Application.Interfaces;
using MediatR;
using RoadRepair.Domain.Entities;

namespace RoadRepair.Application.WorkTimes.Commands.UpdateWorkTime
{
    public class UpdateWorkTimeCommandHandler : IRequestHandler<UpdateWorkTimeCommand, ErrorOr<bool>>
    {
        private IWorkTimeRepository _workTimeRepository;
        private IUnitOfWork _unitOfWork;
        public UpdateWorkTimeCommandHandler(IWorkTimeRepository workTimeRepository, IUnitOfWork unitOfWork)
        {
            _workTimeRepository = workTimeRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(UpdateWorkTimeCommand request, CancellationToken cancellationToken)
        {
            var workTime = await _workTimeRepository.GetWorkTimeByIdAsync(request.WorkTimeId);

            if (workTime == null)
            {
                return Error.NotFound(description: "WorkTime not found");
            }

            workTime.DayOfWork = request.DayOfWork;
            workTime.Hours = request.Hours;
            workTime.RepairEventWorkerId = request.RepairEventWorkerId;

            _workTimeRepository.UpdateWorkTime(workTime);

            await _unitOfWork.CommitChangesAsync();

            return true;
        }
    }
}
