using ErrorOr;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Application.Interfaces;
using MediatR;

namespace RoadRepair.Application.TypesOfRepair.Commands.UpdateTypeOfRepair
{
    public class UpdateTypeOfRepairCommandHandler : IRequestHandler<UpdateTypeOfRepairCommand, ErrorOr<bool>>
    {
        private ITypeOfRepairRepository _typeOfRepairRepository;
        private IUnitOfWork _unitOfWork;
        public UpdateTypeOfRepairCommandHandler(ITypeOfRepairRepository typeOfRepairRepository, IUnitOfWork unitOfWork)
        {
            _typeOfRepairRepository = typeOfRepairRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(UpdateTypeOfRepairCommand request, CancellationToken cancellationToken)
        {
            var typeOfRepair = await _typeOfRepairRepository.GetTypeOfRepairByIdAsync(request.TypeOfRepairId);

            if (typeOfRepair == null)
            {
                return Error.NotFound(description: "TypeOfRepair not found");
            }

            typeOfRepair.Name = request.Name;

            _typeOfRepairRepository.UpdateTypeOfRepair(typeOfRepair);

            await _unitOfWork.CommitChangesAsync();

            return true;
        }
    }
}
