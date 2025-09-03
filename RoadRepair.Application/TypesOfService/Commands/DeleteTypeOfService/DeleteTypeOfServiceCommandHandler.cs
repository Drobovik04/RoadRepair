using ErrorOr;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using RoadRepair.Application.TypesOfService.Commands.DeleteTypeOfService;

namespace RoadRepair.Application.TypesOfRepair.Commands.DeleteTypeOfRepair
{
    public class DeleteTypeOfServiceCommandHandler : IRequestHandler<DeleteTypeOfServiceCommand, ErrorOr<bool>>
    {
        private ITypeOfServiceRepository _typeOfServiceRepository;
        private IUnitOfWork _unitOfWork;
        public DeleteTypeOfServiceCommandHandler(ITypeOfServiceRepository typeOfServiceRepository, IUnitOfWork unitOfWork)
        {
            _typeOfServiceRepository = typeOfServiceRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(DeleteTypeOfServiceCommand request, CancellationToken cancellationToken)
        {
            var typeOfService = await _typeOfServiceRepository.GetTypeOfServiceByIdAsync(request.Id);

            if (typeOfService == null)
            {
                return Error.NotFound(description: "TypeOfService not found");
            }

            _typeOfServiceRepository.DeleteTypeOfService(typeOfService);

            await _unitOfWork.CommitChangesAsync();

            return true;
        }
    }
}
