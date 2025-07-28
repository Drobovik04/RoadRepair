using ErrorOr;
using RoadRepair.Application.Interfaces;
using MediatR;
using RoadRepair.Application.Interfaces.Repositories;

namespace RoadRepair.Application.ContractorServices.Commands.DeleteContractorService
{
    public class DeleteContractorServiceCommandHandler : IRequestHandler<DeleteContractorServiceCommand, ErrorOr<bool>>
    {
        private IContractorServiceRepository _contractorServiceRepository;
        private IUnitOfWork _unitOfWork;
        public DeleteContractorServiceCommandHandler(IContractorServiceRepository contractorServiceRepository, IUnitOfWork unitOfWork)
        {
            _contractorServiceRepository = contractorServiceRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(DeleteContractorServiceCommand request, CancellationToken cancellationToken)
        {
            var contractorService = await _contractorServiceRepository.GetContractorServiceByIdAsync(request.Id);

            if (contractorService == null)
            {
                return Error.NotFound(description: "ContractorService not found");
            }

            _contractorServiceRepository.DeleteContractorService(contractorService);

            await _unitOfWork.CommitChangesAsync();

            return true;
        }
    }
}
