using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RoadRepair.Application.Interfaces;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Application.TypesOfMeasure.Commands.UpdateTypeOfMeasure;
using RoadRepair.Domain.Entities;

namespace RoadRepair.Application.Contractors.Commands.UpdateContractor
{
    public class UpdateContractorCommandHandler : IRequestHandler<UpdateContractorCommand, ErrorOr<bool>>
    {
        private IContractorRepository _contractorRepository;
        private IUnitOfWork _unitOfWork;
        public UpdateContractorCommandHandler(IContractorRepository contractorRepository, IUnitOfWork unitOfWork)
        {
            _contractorRepository = contractorRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(UpdateContractorCommand request, CancellationToken cancellationToken)
        {
            var contractor = await _contractorRepository.GetContractorByIdAsync(request.ContractorId);

            if (contractor == null)
            {
                return Error.NotFound(description: "Contractor not found");
            }

            contractor.Name = request.Name;
            contractor.Address = request.Address;
            contractor.Email = request.Email;
            contractor.ContactPhone = request.ContactPhone;
            contractor.UNP = request.UNP;

            _contractorRepository.UpdateContractor(contractor);

            await _unitOfWork.CommitChangesAsync();

            return true;
        }
    }
}
