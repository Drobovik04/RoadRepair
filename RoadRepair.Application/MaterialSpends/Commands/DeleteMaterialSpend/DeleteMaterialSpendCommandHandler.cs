using ErrorOr;
using MediatR;
using RoadRepair.Application.Interfaces;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Domain.Entities;

namespace RoadRepair.Application.MaterialSpends.Commands.DeleteMaterialSpend
{
    public class DeleteMaterialSpendCommandHandler : IRequestHandler<DeleteMaterialSpendCommand, ErrorOr<bool>>
    {
        private IMaterialSpendRepository _materialSpendRepository;
        private IUnitOfWork _unitOfWork;
        public DeleteMaterialSpendCommandHandler(IMaterialSpendRepository materialSpendRepository, IUnitOfWork unitOfWork)
        {
            _materialSpendRepository = materialSpendRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(DeleteMaterialSpendCommand request, CancellationToken cancellationToken)
        {
            var materialSpend = await _materialSpendRepository.GetMaterialSpendByIdAsync(request.Id);

            if (materialSpend == null)
            {
                return Error.NotFound(description: "MaterialSpend not found");
            }

            _materialSpendRepository.DeleteMaterialSpend(materialSpend);

            await _unitOfWork.CommitChangesAsync();

            return true;
        }
    }
}
