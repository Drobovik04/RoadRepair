using ErrorOr;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Application.Interfaces;
using MediatR;
using RoadRepair.Application.TypesOfMeasure.Commands.UpdateTypeOfMeasure;
using RoadRepair.Domain.Entities;

namespace RoadRepair.Application.MaterialSpends.Commands.UpdateMaterialSpend
{
    public class UpdateMaterialSpendCommandHandler : IRequestHandler<UpdateMaterialSpendCommand, ErrorOr<bool>>
    {
        private IMaterialSpendRepository _materialSpendRepository;
        private IUnitOfWork _unitOfWork;
        public UpdateMaterialSpendCommandHandler(IMaterialSpendRepository materialSpendRepository, IUnitOfWork unitOfWork)
        {
            _materialSpendRepository = materialSpendRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(UpdateMaterialSpendCommand request, CancellationToken cancellationToken)
        {
            var materialSpend = await _materialSpendRepository.GetMaterialSpendByIdAsync(request.MaterialSpendId);

            if (materialSpend == null)
            {
                return Error.NotFound(description: "MaterialSpend not found");
            }

            materialSpend.MaterialId = request.MaterialId;
            materialSpend.Price = request.Price;
            materialSpend.Volume = request.Volume;
            materialSpend.RepairEventId = request.RepairEventId;
            materialSpend.ContractorId = request.ContractorId;

            _materialSpendRepository.UpdateMaterialSpend(materialSpend);

            await _unitOfWork.CommitChangesAsync();

            return true;
        }
    }
}
