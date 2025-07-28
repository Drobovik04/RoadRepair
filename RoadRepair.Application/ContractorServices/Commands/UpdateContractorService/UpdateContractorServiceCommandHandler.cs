using ErrorOr;
using MediatR;
using RoadRepair.Application.ContractorServices.Commands.UpdateContractorService;
using RoadRepair.Application.Interfaces;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Application.TypesOfMeasure.Commands.UpdateTypeOfMeasure;
using RoadRepair.Domain.Entities;

namespace RoadRepair.Application.ContractorServices.Commands.UpdateContractorService
{
    public class UpdateContractorServiceCommandHandler : IRequestHandler<UpdateContractorServiceCommand, ErrorOr<bool>>
    {
        private IContractorServiceRepository _contractorServiceRepository;
        private IUnitOfWork _unitOfWork;
        public UpdateContractorServiceCommandHandler(IContractorServiceRepository contractorServiceRepository, IUnitOfWork unitOfWork)
        {
            _contractorServiceRepository = contractorServiceRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(UpdateContractorServiceCommand request, CancellationToken cancellationToken)
        {
            var contractorService = await _contractorServiceRepository.GetContractorServiceByIdAsync(request.ContractorServiceId);

            if (contractorService == null)
            {
                return Error.NotFound(description: "ContractorService not found");
            }

            contractorService.TypeOfServiceId = request.TypeOfServiceId;
            contractorService.ContractorId = request.ContractorId;
            contractorService.Price = request.Price;
            contractorService.Description = request.Description;
            contractorService.DateOfService = request.DateOfService;
            contractorService.WorkAreaId = request.WorkAreaId;

            _contractorServiceRepository.UpdateContractorService(contractorService);

            await _unitOfWork.CommitChangesAsync();

            return true;
        }
    }
}
