using ErrorOr;
using MediatR;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.MaterialSpends.Queries.GetAllMaterialSpends
{
    public class GetAllMaterialSpendsQueryHandler : IRequestHandler<GetAllMaterialSpendsQuery, ErrorOr<List<MaterialSpend>>>
    {
        private IMaterialSpendRepository _materialSpendRepository;
        public GetAllMaterialSpendsQueryHandler(IMaterialSpendRepository materialSpendRepository)
        {
            _materialSpendRepository = materialSpendRepository;
        }

        public async Task<ErrorOr<List<MaterialSpend>>> Handle(GetAllMaterialSpendsQuery query, CancellationToken cancellationToken)
        {
            var materialSpends = await _materialSpendRepository.GetAllMaterialSpendsByRepairEventIdAsync(query.RepairEventId);

            return materialSpends is null
                ? Error.NotFound(description: "MaterialSpends are not found")
                : materialSpends.ToList();
        }
    }
}
