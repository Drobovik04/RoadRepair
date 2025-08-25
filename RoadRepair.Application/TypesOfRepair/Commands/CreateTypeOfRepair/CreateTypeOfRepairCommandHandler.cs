using ErrorOr;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Application.Interfaces;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace RoadRepair.Application.TypesOfRepair.Commands.CreateTypeOfRepair
{
    public class CreateTypeOfRepairCommandHandler : IRequestHandler<CreateTypeOfRepairCommand, ErrorOr<TypeOfRepair>>
    {
        private ITypeOfRepairRepository _typeOfRepairRepository;
        private IUnitOfWork _unitOfWork;
        public CreateTypeOfRepairCommandHandler(ITypeOfRepairRepository typeOfRepairRepository, IUnitOfWork unitOfWork)
        {
            _typeOfRepairRepository = typeOfRepairRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<TypeOfRepair>> Handle(CreateTypeOfRepairCommand request, CancellationToken cancellationToken)
        {
            var typeOfRepair = new TypeOfRepair
            {
                Name = request.Name,
            };

            await _typeOfRepairRepository.AddTypeOfRepairAsync(typeOfRepair);

            await _unitOfWork.CommitChangesAsync();

            return typeOfRepair;
        }
    }
}
