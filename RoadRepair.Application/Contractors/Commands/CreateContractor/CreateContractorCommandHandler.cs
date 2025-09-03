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

namespace RoadRepair.Application.Contractors.Commands.CreateContractor
{
    public class CreateContractorCommandHandler : IRequestHandler<CreateContractorCommand, ErrorOr<Contractor>>
    {
        private IContractorRepository _contractorRepository;
        private IUnitOfWork _unitOfWork;
        public CreateContractorCommandHandler(IContractorRepository contractorRepository, IUnitOfWork unitOfWork)
        {
            _contractorRepository = contractorRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<Contractor>> Handle(CreateContractorCommand request, CancellationToken cancellationToken)
        {
            var contractor = new Contractor
            {
                Name = request.Name,
                Address = request.Address,
                Email = request.Email,
                ContactPhone = request.ContactPhone,
                UNP = request.UNP
            };

            await _contractorRepository.AddContractorAsync(contractor);

            await _unitOfWork.CommitChangesAsync();

            return contractor;
        }
    }
}
