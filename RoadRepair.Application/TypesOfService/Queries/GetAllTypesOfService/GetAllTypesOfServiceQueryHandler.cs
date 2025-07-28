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

namespace RoadRepair.Application.TypesOfService.Queries.GetAllTypesOfService
{
    public class GetAllTypesOfServiceQueryHandler : IRequestHandler<GetAllTypesOfServiceQuery, ErrorOr<List<TypeOfService>>>
    {
        private ITypeOfServiceRepository _typeOfServiceRepository;
        public GetAllTypesOfServiceQueryHandler(ITypeOfServiceRepository typeOfServiceRepository)
        {
            _typeOfServiceRepository = typeOfServiceRepository;
        }

        public async Task<ErrorOr<List<TypeOfService>>> Handle(GetAllTypesOfServiceQuery query, CancellationToken cancellationToken)
        {
            var types = await _typeOfServiceRepository.GetAllTypesOfServiceAsync();

            return types is null
                ? Error.NotFound(description: "TypesOfService not found")
                : types.ToList();
        }
    }
}
