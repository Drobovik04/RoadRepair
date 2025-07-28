using ErrorOr;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Application.Interfaces;
using MediatR;

namespace RoadRepair.Application.Positions.Commands.UpdatePosition
{
    public class UpdatePositionCommandHandler : IRequestHandler<UpdatePositionCommand, ErrorOr<bool>>
    {
        private IPositionRepository _positionRepository;
        private IUnitOfWork _unitOfWork;
        public UpdatePositionCommandHandler(IPositionRepository positionRepository, IUnitOfWork unitOfWork)
        {
            _positionRepository = positionRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(UpdatePositionCommand request, CancellationToken cancellationToken)
        {
            var position = await _positionRepository.GetPositionByIdAsync(request.PositionId);

            if (position == null)
            {
                return Error.NotFound(description: "Position not found");
            }

            position.Name = request.Name;

            _positionRepository.UpdatePosition(position);

            await _unitOfWork.CommitChangesAsync();

            return true;
        }
    }
}
