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

namespace RoadRepair.Application.Contractors.Queries.GetAllContractors
{
    public class GetAllContractorsQueryHandler : IRequestHandler<GetAllContractorsQuery, ErrorOr<List<Contractor>>>
    {
        private IContractorRepository _contractorRepository;
        public GetAllContractorsQueryHandler(IContractorRepository contractorRepository)
        {
            _contractorRepository = contractorRepository;
        }

        public async Task<ErrorOr<List<Contractor>>> Handle(GetAllContractorsQuery query, CancellationToken cancellationToken)
        {
            var contractors = await _contractorRepository.GetAllContractorsAsync();

            return contractors is null
                ? Error.NotFound(description: "Contractors not found")
                : contractors.ToList();
        }
    }
}
