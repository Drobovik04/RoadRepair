using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RoadRepair.Application.Interfaces;
using RoadRepair.Application.Interfaces.Repositories;

namespace RoadRepair.Application.Contractors.Commands.DeleteContractor
{
    public class DeleteContractorCommandHandler : IRequestHandler<DeleteContractorCommand, ErrorOr<bool>>
    {
        private IContractorRepository _contractorRepository;
        private IUnitOfWork _unitOfWork;
        public DeleteContractorCommandHandler(IContractorRepository contractorRepository, IUnitOfWork unitOfWork)
        {
            _contractorRepository = contractorRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(DeleteContractorCommand request, CancellationToken cancellationToken)
        {
            var contractor = await _contractorRepository.GetContractorByIdAsync(request.Id);

            if (contractor == null)
            {
                return Error.NotFound(description: "Contractor not found");
            }

            _contractorRepository.DeleteContractor(contractor);

            await _unitOfWork.CommitChangesAsync();

            return true;
        }
    }
}
