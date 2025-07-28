using ErrorOr;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Application.Interfaces;
using RoadRepair.Application.Organizations.Commands.CreateOrganization;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace RoadRepair.Application.WorkTimes.Commands.CreateWorkTime
{
    public class CreateWorkTimeCommandHandler : IRequestHandler<CreateWorkTimeCommand, ErrorOr<WorkTime>>
    {
        private IWorkTimeRepository _workTimeRepository;
        private IUnitOfWork _unitOfWork;
        public CreateWorkTimeCommandHandler(IWorkTimeRepository workTimeRepository, IUnitOfWork unitOfWork)
        {
            _workTimeRepository = workTimeRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<WorkTime>> Handle(CreateWorkTimeCommand request, CancellationToken cancellationToken)
        {
            var workTime = new WorkTime
            {
                DayOfWork = request.DayOfWork,
                Hours = request.Hours,
                RepairEventWorkerId = request.RepairEventWorkerId,
            };

            await _workTimeRepository.AddWorkTimeAsync(workTime);

            await _unitOfWork.CommitChangesAsync();

            return workTime;
        }
    }
}
