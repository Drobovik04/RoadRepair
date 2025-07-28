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

namespace RoadRepair.Application.TypesOfService.Queries.GetTypeOfService
{
    public class GetTypeOfServiceQueryHandler : IRequestHandler<GetTypeOfServiceQuery, ErrorOr<TypeOfService>>
    {
        private ITypeOfServiceRepository _typeOfServiceRepository;
        public GetTypeOfServiceQueryHandler(ITypeOfServiceRepository typeOfServiceRepository)
        {
            _typeOfServiceRepository = typeOfServiceRepository;
        }

        public async Task<ErrorOr<TypeOfService>> Handle(GetTypeOfServiceQuery query, CancellationToken cancellationToken)
        {
            var typeOfService = await _typeOfServiceRepository.GetTypeOfServiceByIdAsync(query.TypeOfServiceId);

            return typeOfService is null
                ? Error.NotFound(description: "TypeOfService not found")
                : typeOfService;
        }
    }
}
