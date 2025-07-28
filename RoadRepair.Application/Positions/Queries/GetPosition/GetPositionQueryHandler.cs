using ErrorOr;
using MediatR;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Application.Organizations.Queries.GetOrganization;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Positions.Queries.GetPosition
{
    public class GetPositionQueryHandler : IRequestHandler<GetPositionQuery, ErrorOr<Position>>
    {
        private IPositionRepository _positionsRepository;
        public GetPositionQueryHandler(IPositionRepository positionsRepository)
        {
            _positionsRepository = positionsRepository;
        }

        public async Task<ErrorOr<Position>> Handle(GetPositionQuery query, CancellationToken cancellationToken)
        {
            var position = await _positionsRepository.GetPositionByIdAsync(query.PositionId);

            return position is null
                ? Error.NotFound(description: "Position not found")
                : position;
        }
    }
}
