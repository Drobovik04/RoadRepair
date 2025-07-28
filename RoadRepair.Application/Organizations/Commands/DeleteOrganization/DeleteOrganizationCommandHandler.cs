using ErrorOr;
using MediatR;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Application.Interfaces;
using RoadRepair.Application.Organizations.Commands.UpdateOrganization;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Organizations.Commands.DeleteOrganization
{
    public class DeleteOrganizationCommandHandler : IRequestHandler<DeleteOrganizationCommand, ErrorOr<bool>>
    {
        private IOrganizationRepository _organizationRepository;
        private IUnitOfWork _unitOfWork;
        public DeleteOrganizationCommandHandler(IOrganizationRepository organizationRepository, IUnitOfWork unitOfWork)
        {
            _organizationRepository = organizationRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(DeleteOrganizationCommand request, CancellationToken cancellationToken)
        {
            var organization = await _organizationRepository.GetOrganizationByIdAsync(request.Id);

            if (organization == null)
            {
                return Error.NotFound(description: "Organization not found");
            }

            _organizationRepository.DeleteOrganization(organization);

            await _unitOfWork.CommitChangesAsync();

            return true;
        }
    }
}
