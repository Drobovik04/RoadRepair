using ErrorOr;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Application.Interfaces;
using RoadRepair.Application.Organizations.Commands.CreateOrganization;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace RoadRepair.Application.TypesOfService.Commands.CreateTypeOfService
{
    public class CreateTypeOfServiceCommandHandler : IRequestHandler<CreateTypeOfServiceCommand, ErrorOr<TypeOfService>>
    {
        private ITypeOfServiceRepository _typeOfServiceRepository;
        private IUnitOfWork _unitOfWork;
        public CreateTypeOfServiceCommandHandler(ITypeOfServiceRepository typeOfServiceRepository, IUnitOfWork unitOfWork)
        {
            _typeOfServiceRepository = typeOfServiceRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<TypeOfService>> Handle(CreateTypeOfServiceCommand request, CancellationToken cancellationToken)
        {
            var typeOfService = new TypeOfService
            {
                Name = request.Name,
            };

            await _typeOfServiceRepository.AddTypeOfServiceAsync(typeOfService);

            await _unitOfWork.CommitChangesAsync();

            return typeOfService;
        }
    }
}
