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

namespace RoadRepair.Application.Contractors.Queries.GetContractor
{
    public class GetContractorQueryHandler : IRequestHandler<GetContractorQuery, ErrorOr<Contractor>>
    {
        private IContractorRepository _contractorRepository;
        public GetContractorQueryHandler(IContractorRepository contractorRepository)
        {
            _contractorRepository = contractorRepository;
        }

        public async Task<ErrorOr<Contractor>> Handle(GetContractorQuery query, CancellationToken cancellationToken)
        {
            var contractor = await _contractorRepository.GetContractorByIdAsync(query.ContractorId);

            return contractor is null
                ? Error.NotFound(description: "Contractor not found")
                : contractor;
        }
    }
}
