using ErrorOr;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Application.Interfaces;
using MediatR;

namespace RoadRepair.Application.TypesOfService.Commands.UpdateTypeOfService
{
    public class UpdateTypeOfServiceCommandHandler : IRequestHandler<UpdateTypeOfServiceCommand, ErrorOr<bool>>
    {
        private ITypeOfServiceRepository _typeOfServiceRepository;
        private IUnitOfWork _unitOfWork;
        public UpdateTypeOfServiceCommandHandler(ITypeOfServiceRepository typeOfServiceRepository, IUnitOfWork unitOfWork)
        {
            _typeOfServiceRepository = typeOfServiceRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(UpdateTypeOfServiceCommand request, CancellationToken cancellationToken)
        {
            var typeOfService = await _typeOfServiceRepository.GetTypeOfServiceByIdAsync(request.TypeOfServiceId);

            if (typeOfService == null)
            {
                return Error.NotFound(description: "TypeOfService not found");
            }

            typeOfService.Name = request.Name;

            _typeOfServiceRepository.UpdateTypeOfService(typeOfService);

            await _unitOfWork.CommitChangesAsync();

            return true;
        }
    }
}
