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

namespace RoadRepair.Application.Materials.Commands.CreateMaterial
{
    public class CreateMaterialCommandHandler : IRequestHandler<CreateMaterialCommand, ErrorOr<Material>>
    {
        private IMaterialRepository _materialRepository;
        private IUnitOfWork _unitOfWork;
        public CreateMaterialCommandHandler(IMaterialRepository materialRepository, IUnitOfWork unitOfWork)
        {
            _materialRepository = materialRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<Material>> Handle(CreateMaterialCommand request, CancellationToken cancellationToken)
        {
            var material = new Material
            {
                Name = request.Name,
                TypeOfMeasureId = request.TypeOfMeasureId,
            };

            await _materialRepository.AddMaterialAsync(material);

            await _unitOfWork.CommitChangesAsync();

            return material;
        }
    }
}
