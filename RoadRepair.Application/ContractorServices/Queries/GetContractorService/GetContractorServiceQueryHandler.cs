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

namespace RoadRepair.Application.ContractorServices.Queries.GetContractorService
{
    public class GetContractorServiceQueryHandler : IRequestHandler<GetContractorServiceQuery, ErrorOr<ContractorService>>
    {
        private IContractorServiceRepository _contractorServiceRepository;
        public GetContractorServiceQueryHandler(IContractorServiceRepository contractorServiceRepository)
        {
            _contractorServiceRepository = contractorServiceRepository;
        }

        public async Task<ErrorOr<ContractorService>> Handle(GetContractorServiceQuery query, CancellationToken cancellationToken)
        {
            var contractorService = await _contractorServiceRepository.GetContractorServiceByIdAsync(query.ContractorServiceId);

            return contractorService is null
                ? Error.NotFound(description: "ContractorService not found")
                : contractorService;
        }
    }
}
