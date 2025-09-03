using ErrorOr;
using MediatR;
using RoadRepair.Application.Interfaces;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Application.TypesOfService.Commands.DeleteTypeOfService;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.WorkAreas.Commands.DeleteWorkArea
{
    public class DeleteWorkAreaCommandHandler : IRequestHandler<DeleteWorkAreaCommand, ErrorOr<bool>>
    {
        private IWorkAreaRepository _workAreaRepository;
        private IUnitOfWork _unitOfWork;
        public DeleteWorkAreaCommandHandler(IWorkAreaRepository workAreaRepository, IUnitOfWork unitOfWork)
        {
            _workAreaRepository = workAreaRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(DeleteWorkAreaCommand request, CancellationToken cancellationToken)
        {
            var workArea = await _workAreaRepository.GetWorkAreaByIdAsync(request.Id);

            if (workArea == null)
            {
                return Error.NotFound(description: "WorkArea not found");
            }

            _workAreaRepository.DeleteWorkArea(workArea);

            await _unitOfWork.CommitChangesAsync();

            return true;
        }
    }
}
