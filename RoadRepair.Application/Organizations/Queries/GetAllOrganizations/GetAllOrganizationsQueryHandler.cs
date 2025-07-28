using ErrorOr;
using MediatR;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Application.Organizations.Commands.CreateOrganization;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Organizations.Queries.GetAllOrganizations
{
    public class GetAllOrganizationsQueryHandler : IRequestHandler<GetAllOrganizationsQuery, ErrorOr<List<Organization>>>
    {
        private IOrganizationRepository _organizationsRepository;
        public GetAllOrganizationsQueryHandler(IOrganizationRepository organizationsRepository)
        {
            _organizationsRepository = organizationsRepository;
        }

        public async Task<ErrorOr<List<Organization>>> Handle(GetAllOrganizationsQuery query, CancellationToken cancellationToken)
        {
            var organizations = await _organizationsRepository.GetAllOrganizationsAsync();

            return organizations is null
                ? Error.NotFound(description: "Organization not found")
                : organizations.ToList();
        }
    }
}
