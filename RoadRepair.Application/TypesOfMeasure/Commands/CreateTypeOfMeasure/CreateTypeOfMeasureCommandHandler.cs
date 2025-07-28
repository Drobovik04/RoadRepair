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

namespace RoadRepair.Application.TypesOfMeasure.Commands.CreateTypeOfMeasure
{
    public class CreateTypeOfMeasureCommandHandler : IRequestHandler<CreateTypeOfMeasureCommand, ErrorOr<TypeOfMeasure>>
    {
        private ITypeOfMeasureRepository _typeOfMeasureRepository;
        private IUnitOfWork _unitOfWork;
        public CreateTypeOfMeasureCommandHandler(ITypeOfMeasureRepository typeOfMeasureRepository, IUnitOfWork unitOfWork)
        {
            _typeOfMeasureRepository = typeOfMeasureRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<TypeOfMeasure>> Handle(CreateTypeOfMeasureCommand request, CancellationToken cancellationToken)
        {
            var typeOfMeasure = new TypeOfMeasure
            {
                Name = request.Name,
            };

            await _typeOfMeasureRepository.AddTypeOfMeasureAsync(typeOfMeasure);

            await _unitOfWork.CommitChangesAsync();

            return typeOfMeasure;
        }
    }
}
