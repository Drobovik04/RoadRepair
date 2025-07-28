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

namespace RoadRepair.Application.Organizations.Queries.GetOrganization
{
    public class GetOrganizationQueryHandler : IRequestHandler<GetOrganizationQuery, ErrorOr<Organization>>
    {
        private IOrganizationRepository _organizationsRepository;
        public GetOrganizationQueryHandler(IOrganizationRepository organizationsRepository)
        {
            _organizationsRepository = organizationsRepository;
        }

        public async Task<ErrorOr<Organization>> Handle(GetOrganizationQuery query, CancellationToken cancellationToken)
        {
            var organization = await _organizationsRepository.GetOrganizationByIdAsync(query.OrganizationId);

            return organization is null
                ? Error.NotFound(description: "Organization not found")
                : organization;
        }
    }
}
