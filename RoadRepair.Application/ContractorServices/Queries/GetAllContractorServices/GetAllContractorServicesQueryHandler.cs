using ErrorOr;
using MediatR;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.ContractorServices.Queries.GetAllContractorServices
{
    public class GetAllContractorServicesQueryHandler : IRequestHandler<GetAllContractorServicesQuery, ErrorOr<List<ContractorService>>>
    {
        private IContractorServiceRepository _contractorServiceRepository;
        public GetAllContractorServicesQueryHandler(IContractorServiceRepository contractorServiceRepository)
        {
            _contractorServiceRepository = contractorServiceRepository;
        }

        public async Task<ErrorOr<List<ContractorService>>> Handle(GetAllContractorServicesQuery query, CancellationToken cancellationToken)
        {
            var contractorServices = await _contractorServiceRepository.GetAllContractorServicesByWorkAreaIdAsync(query.workAreaId);

            return contractorServices is null
                ? Error.NotFound(description: "ContractorServices not found")
                : contractorServices.ToList();
        }
    }
}
