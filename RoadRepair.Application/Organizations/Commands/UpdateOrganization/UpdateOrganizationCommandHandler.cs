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

namespace RoadRepair.Application.Organizations.Commands.UpdateOrganization
{
    public class UpdateOrganizationCommandHandler : IRequestHandler<UpdateOrganizationCommand, ErrorOr<bool>>
    {
        private IOrganizationRepository _organizationRepository;
        private IUnitOfWork _unitOfWork;
        public UpdateOrganizationCommandHandler(IOrganizationRepository organizationRepository, IUnitOfWork unitOfWork)
        {
            _organizationRepository = organizationRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(UpdateOrganizationCommand request, CancellationToken cancellationToken)
        {
            var organization = await _organizationRepository.GetOrganizationByIdAsync(request.OrganizationId);

            if (organization == null)
            {
                return Error.NotFound(description: "Organization not found");
            }

            organization.Name = request.Name;
            organization.Address = request.Address;
            organization.ContactPhone = request.ContactPhone;
            organization.Email = request.Email;
            organization.UNP = request.UNP;
            organization.ParentId = request.ParentId;

            _organizationRepository.UpdateOrganization(organization);

            await _unitOfWork.CommitChangesAsync();

            return true;
        }
    }
}
