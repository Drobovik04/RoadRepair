using ErrorOr;
using MediatR;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.TypesOfRepair.Queries.GetAllTypesOfRepair
{
    public class GetAllTypesOfRepairQueryHandler : IRequestHandler<GetAllTypesOfRepairQuery, ErrorOr<List<TypeOfRepair>>>
    {
        private ITypeOfRepairRepository _typeOfRepairRepository;
        public GetAllTypesOfRepairQueryHandler(ITypeOfRepairRepository typeOfRepairRepository)
        {
            _typeOfRepairRepository = typeOfRepairRepository;
        }

        public async Task<ErrorOr<List<TypeOfRepair>>> Handle(GetAllTypesOfRepairQuery query, CancellationToken cancellationToken)
        {
            var types = await _typeOfRepairRepository.GetAllTypesOfRepairAsync();

            return types is null
                ? Error.NotFound(description: "TypesOfRepair not found")
                : types.ToList();
        }
    }
}
