using ErrorOr;
using RoadRepair.Application.Interfaces;
using MediatR;
using RoadRepair.Application.Interfaces.Repositories;

namespace RoadRepair.Application.Materials.Commands.DeleteMaterial
{
    public class DeleteMaterialCommandHandler : IRequestHandler<DeleteMaterialCommand, ErrorOr<bool>>
    {
        private IMaterialRepository _materialRepository;
        private IUnitOfWork _unitOfWork;
        public DeleteMaterialCommandHandler(IMaterialRepository materialRepository, IUnitOfWork unitOfWork)
        {
            _materialRepository = materialRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(DeleteMaterialCommand request, CancellationToken cancellationToken)
        {
            var material = await _materialRepository.GetMaterialByIdAsync(request.Id);

            if (material == null)
            {
                return Error.NotFound(description: "Material not found");
            }

            _materialRepository.DeleteMaterial(material);

            await _unitOfWork.CommitChangesAsync();

            return true;
        }
    }
}
