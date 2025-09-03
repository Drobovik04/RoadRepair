using ErrorOr;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Application.Interfaces;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace RoadRepair.Application.ContractorServices.Commands.CreateContractorService
{
    public class CreateContractorServiceCommandHandler : IRequestHandler<CreateContractorServiceCommand, ErrorOr<ContractorService>>
    {
        private IContractorServiceRepository _contractorServiceRepository;
        private IUnitOfWork _unitOfWork;
        public CreateContractorServiceCommandHandler(IContractorServiceRepository contractorServiceRepository, IUnitOfWork unitOfWork)
        {
            _contractorServiceRepository = contractorServiceRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<ContractorService>> Handle(CreateContractorServiceCommand request, CancellationToken cancellationToken)
        {
            var contractorService = new ContractorService
            {
                TypeOfServiceId = request.TypeOfServiceId,
                ContractorId = request.ContractorId,
                Price = request.Price,
                Description = request.Description,
                DateOfService = request.DateOfService,
                WorkAreaId = request.WorkAreaId,
            };

            await _contractorServiceRepository.AddContractorServiceAsync(contractorService);

            await _unitOfWork.CommitChangesAsync();

            return contractorService;
        }
    }
}
