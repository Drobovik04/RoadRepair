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

namespace RoadRepair.Application.TypesOfMeasure.Queries.GetTypeOfMeasure
{
    public class GetTypeOfMeasureQueryHandler : IRequestHandler<GetTypeOfMeasureQuery, ErrorOr<TypeOfMeasure>>
    {
        private ITypeOfMeasureRepository _typeOfMeasureRepository;
        public GetTypeOfMeasureQueryHandler(ITypeOfMeasureRepository typeOfMeasureRepository)
        {
            _typeOfMeasureRepository = typeOfMeasureRepository;
        }

        public async Task<ErrorOr<TypeOfMeasure>> Handle(GetTypeOfMeasureQuery query, CancellationToken cancellationToken)
        {
            var type = await _typeOfMeasureRepository.GetTypeOfMeasureByIdAsync(query.TypeOfMeasureId);

            return type is null
                ? Error.NotFound(description: "TypeOfMeasure not found")
                : type;
        }
    }
}
