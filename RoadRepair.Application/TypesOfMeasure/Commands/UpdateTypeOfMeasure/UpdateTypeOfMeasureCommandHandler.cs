using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RoadRepair.Application.Errors;
using RoadRepair.Application.Interfaces;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Application.TypesOfMeasure.Commands.UpdateTypeOfMeasure;

namespace RoadRepair.Application.TypesOfMeasure.Commands.UpdateTypeOfMeasure
{
    public class UpdateTypeOfMeasureCommandHandler : IRequestHandler<UpdateTypeOfMeasureCommand, ErrorOr<bool>>
    {
        private ITypeOfMeasureRepository _typeOfMeasureRepository;
        private IUnitOfWork _unitOfWork;
        public UpdateTypeOfMeasureCommandHandler(ITypeOfMeasureRepository typeOfMeasureRepository, IUnitOfWork unitOfWork)
        {
            _typeOfMeasureRepository = typeOfMeasureRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(UpdateTypeOfMeasureCommand request, CancellationToken cancellationToken)
        {
            var typeOfMeasure = await _typeOfMeasureRepository.GetTypeOfMeasureByIdAsync(request.TypeOfMeasureId);

            if (typeOfMeasure == null)
            {
                return Error.NotFound(description: "TypeOfMeasure not found");
            }

            typeOfMeasure.Name = request.Name;
            typeOfMeasure.ShortName = request.ShortName;

            _typeOfMeasureRepository.UpdateTypeOfMeasure(typeOfMeasure);

            await _unitOfWork.CommitChangesAsync();

            return true;
        }
    }
}
