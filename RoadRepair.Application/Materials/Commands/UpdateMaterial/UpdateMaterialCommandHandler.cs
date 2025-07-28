using ErrorOr;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Application.Interfaces;
using MediatR;
using RoadRepair.Application.TypesOfMeasure.Commands.UpdateTypeOfMeasure;
using RoadRepair.Domain.Entities;

namespace RoadRepair.Application.Materials.Commands.UpdateMaterial
{
    public class UpdateMaterialCommandHandler : IRequestHandler<UpdateMaterialCommand, ErrorOr<bool>>
    {
        private IMaterialRepository _materialRepository;
        private IUnitOfWork _unitOfWork;
        public UpdateMaterialCommandHandler(IMaterialRepository materialRepository, IUnitOfWork unitOfWork)
        {
            _materialRepository = materialRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(UpdateMaterialCommand request, CancellationToken cancellationToken)
        {
            var material = await _materialRepository.GetMaterialByIdAsync(request.MaterialId);

            if (material == null)
            {
                return Error.NotFound(description: "Material not found");
            }

            material.Name = request.Name;
            material.TypeOfMeasureId = request.TypeOfMeasureId;
            material.OrganizationId = request.OrganizationId;

            _materialRepository.UpdateMaterial(material);

            await _unitOfWork.CommitChangesAsync();

            return true;
        }
    }
}
