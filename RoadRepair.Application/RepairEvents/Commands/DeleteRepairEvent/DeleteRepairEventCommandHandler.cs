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

namespace RoadRepair.Application.RepairEvents.Commands.DeleteRepairEvent
{
    public class DeleteRepairEventCommandHandler : IRequestHandler<DeleteRepairEventCommand, ErrorOr<bool>>
    {
        private IRepairEventRepository _repairEventRepository;
        private IUnitOfWork _unitOfWork;
        public DeleteRepairEventCommandHandler(IRepairEventRepository repairEventRepository, IUnitOfWork unitOfWork)
        {
            _repairEventRepository = repairEventRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(DeleteRepairEventCommand request, CancellationToken cancellationToken)
        {
            var repairEvent = await _repairEventRepository.GetRepairEventByIdAsync(request.Id);

            if (repairEvent == null)
            {
                return Error.NotFound(description: "RepairEvent not found");
            }

            _repairEventRepository.DeleteRepairEvent(repairEvent);

            await _unitOfWork.CommitChangesAsync();

            return true;
        }
    }
}
