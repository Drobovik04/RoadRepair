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

namespace RoadRepair.Application.MaterialSpends.Commands.CreateMaterialSpend
{
    public class CreateMaterialSpendCommandHandler : IRequestHandler<CreateMaterialSpendCommand, ErrorOr<MaterialSpend>>
    {
        private IMaterialSpendRepository _materialSpendRepository;
        private IUnitOfWork _unitOfWork;
        public CreateMaterialSpendCommandHandler(IMaterialSpendRepository materialSpendRepository, IUnitOfWork unitOfWork)
        {
            _materialSpendRepository = materialSpendRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<MaterialSpend>> Handle(CreateMaterialSpendCommand request, CancellationToken cancellationToken)
        {
            var materialSpend = new MaterialSpend
            {
                MaterialId = request.MaterialId,
                Price = request.Price,
                Volume = request.Volume,
                RepairEventId = request.RepairEventId,
                ContractorId = request.ContractorId
            };

            await _materialSpendRepository.AddMaterialSpendAsync(materialSpend);

            await _unitOfWork.CommitChangesAsync();

            return materialSpend;
        }
    }
}
