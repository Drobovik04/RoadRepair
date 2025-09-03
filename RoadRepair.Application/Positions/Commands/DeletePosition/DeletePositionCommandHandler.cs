using ErrorOr;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace RoadRepair.Application.Positions.Commands.DeletePosition
{
    public class DeletePositionCommandHandler : IRequestHandler<DeletePositionCommand, ErrorOr<bool>>
    {
        private IPositionRepository _positionRepository;
        private IUnitOfWork _unitOfWork;
        public DeletePositionCommandHandler(IPositionRepository positionRepository, IUnitOfWork unitOfWork)
        {
            _positionRepository = positionRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(DeletePositionCommand request, CancellationToken cancellationToken)
        {
            var position = await _positionRepository.GetPositionByIdAsync(request.Id);

            if (position == null)
            {
                return Error.NotFound(description: "Position not found");
            }

            _positionRepository.DeletePosition(position);

            await _unitOfWork.CommitChangesAsync();

            return true;
        }
    }
}
