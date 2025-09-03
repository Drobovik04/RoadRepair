using ErrorOr;
using MediatR;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.MaterialSpends.Queries.GetMaterialSpend
{
    public class GetMaterialSpendQueryHandler : IRequestHandler<GetMaterialSpendQuery, ErrorOr<MaterialSpend>>
    {
        private IMaterialSpendRepository _materialSpendRepository;
        public GetMaterialSpendQueryHandler(IMaterialSpendRepository materialSpendRepository)
        {
            _materialSpendRepository = materialSpendRepository;
        }

        public async Task<ErrorOr<MaterialSpend>> Handle(GetMaterialSpendQuery query, CancellationToken cancellationToken)
        {
            var materialSpend = await _materialSpendRepository.GetMaterialSpendByIdAsync(query.MaterialSpendId);

            return materialSpend is null
                ? Error.NotFound(description: "MaterialSpend not found")
                : materialSpend;
        }
    }
}
