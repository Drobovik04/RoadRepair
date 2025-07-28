using ErrorOr;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Application.Interfaces;
using RoadRepair.Application.Organizations.Commands.DeleteOrganization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace RoadRepair.Application.TypesOfMeasure.Commands.DeleteTypeOfMeasure
{
    public class DeleteTypeOfMeasureCommandHandler : IRequestHandler<DeleteTypeOfMeasureCommand, ErrorOr<bool>>
    {
        private ITypeOfMeasureRepository _typeOfMeasureRepository;
        private IUnitOfWork _unitOfWork;
        public DeleteTypeOfMeasureCommandHandler(ITypeOfMeasureRepository typeOfMeasureRepository, IUnitOfWork unitOfWork)
        {
            _typeOfMeasureRepository = typeOfMeasureRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(DeleteTypeOfMeasureCommand request, CancellationToken cancellationToken)
        {
            var typeOfMeasure = await _typeOfMeasureRepository.GetTypeOfMeasureByIdAsync(request.Id);

            if (typeOfMeasure == null)
            {
                return Error.NotFound(description: "TypeOfMeasure not found");
            }

            _typeOfMeasureRepository.DeleteTypeOfMeasure(typeOfMeasure);

            await _unitOfWork.CommitChangesAsync();

            return true;
        }
    }
}
