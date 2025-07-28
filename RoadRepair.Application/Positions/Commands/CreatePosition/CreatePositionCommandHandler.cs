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

namespace RoadRepair.Application.Positions.Commands.CreatePosition
{
    public class CreatePositionCommandHandler : IRequestHandler<CreatePositionCommand, ErrorOr<Position>>
    {
        private IPositionRepository _positionRepository;
        private IUnitOfWork _unitOfWork;
        public CreatePositionCommandHandler(IPositionRepository positionRepository, IUnitOfWork unitOfWork)
        {
            _positionRepository = positionRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<Position>> Handle(CreatePositionCommand request, CancellationToken cancellationToken)
        {
            var position = new Position
            {
                Name = request.Name,
            };

            await _positionRepository.AddPositionAsync(position);

            await _unitOfWork.CommitChangesAsync();

            return position;
        }
    }
}
