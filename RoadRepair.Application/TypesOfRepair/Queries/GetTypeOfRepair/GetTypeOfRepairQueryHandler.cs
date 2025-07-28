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

namespace RoadRepair.Application.TypesOfRepair.Queries.GetTypeOfRepair
{
    public class GetTypeOfRepairQueryHandler : IRequestHandler<GetTypeOfRepairQuery, ErrorOr<TypeOfRepair>>
    {
        private ITypeOfRepairRepository _typeOfRepairRepository;
        public GetTypeOfRepairQueryHandler(ITypeOfRepairRepository typeOfRepairRepository)
        {
            _typeOfRepairRepository = typeOfRepairRepository;
        }

        public async Task<ErrorOr<TypeOfRepair>> Handle(GetTypeOfRepairQuery query, CancellationToken cancellationToken)
        {
            var position = await _typeOfRepairRepository.GetTypeOfRepairByIdAsync(query.TypeOfRepairId);

            return position is null
                ? Error.NotFound(description: "TypeOfRepair not found")
                : position;
        }
    }
}
