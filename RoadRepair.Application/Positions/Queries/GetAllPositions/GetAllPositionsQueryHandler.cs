using ErrorOr;
using MediatR;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Positions.Queries.GetAllPositions
{
    public class GetAllContractorsQueryHandler : IRequestHandler<GetAllPositionsQuery, ErrorOr<List<Position>>>
    {
        private IPositionRepository _positionRepository;
        public GetAllContractorsQueryHandler(IPositionRepository positionRepository)
        {
            _positionRepository = positionRepository;
        }

        public async Task<ErrorOr<List<Position>>> Handle(GetAllPositionsQuery query, CancellationToken cancellationToken)
        {
            var contractors = await _positionRepository.GetAllPositionsAsync();

            return contractors is null
                ? Error.NotFound(description: "Positions not found")
                : contractors.ToList();
        }
    }
}
