using ErrorOr;
using MediatR;
using RoadRepair.Application.Interfaces;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Organizations.Commands.CreateOrganization
{
    public class CreateOrganizationCommandHandler : IRequestHandler<CreateOrganizationCommand, ErrorOr<Organization>>
    {
        private IOrganizationRepository _organizationRepository;
        private IUnitOfWork _unitOfWork;
        public CreateOrganizationCommandHandler(IOrganizationRepository organizationRepository, IUnitOfWork unitOfWork)
        {
            _organizationRepository = organizationRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<Organization>> Handle(CreateOrganizationCommand request, CancellationToken cancellationToken)
        {
            var organization = new Organization
            {
                Name = request.Name,
                Address = request.Address,
                ContactPhone = request.ContactPhone,
                Email = request.Email,
                UNP = request.UNP,
                ParentId = request.ParentId
            };

            await _organizationRepository.AddOrganizationAsync(organization);

            await _unitOfWork.CommitChangesAsync();

            return organization;
        }
    }
}
