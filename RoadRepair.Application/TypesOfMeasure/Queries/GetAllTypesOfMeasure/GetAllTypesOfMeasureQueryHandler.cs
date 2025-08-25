using ErrorOr;
using MediatR;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.TypesOfMeasure.Queries.GetAllTypesOfMeasure
{
    public class GetAllTypesOfMeasureQueryHandler : IRequestHandler<GetAllTypesOfMeasureQuery, ErrorOr<List<TypeOfMeasure>>>
    {
        private ITypeOfMeasureRepository _typeOfMeasureRepository;
        public GetAllTypesOfMeasureQueryHandler(ITypeOfMeasureRepository typeOfMeasureRepository)
        {
            _typeOfMeasureRepository = typeOfMeasureRepository;
        }

        public async Task<ErrorOr<List<TypeOfMeasure>>> Handle(GetAllTypesOfMeasureQuery query, CancellationToken cancellationToken)
        {
            var types = await _typeOfMeasureRepository.GetAllTypesOfMeasureAsync();

            return types is null
                ? Error.NotFound(description: "TypesOfMeasure not found")
                : types.ToList();
        }
    }
}
