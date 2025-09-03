using ErrorOr;
using MediatR;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Materials.Queries.GetMaterial
{
    public class GetMaterialQueryHandler : IRequestHandler<GetMaterialQuery, ErrorOr<Material>>
    {
        private IMaterialRepository _materialRepository;
        public GetMaterialQueryHandler(IMaterialRepository materialRepository)
        {
            _materialRepository = materialRepository;
        }

        public async Task<ErrorOr<Material>> Handle(GetMaterialQuery query, CancellationToken cancellationToken)
        {
            var material = await _materialRepository.GetMaterialByIdAsync(query.MaterialId);

            return material is null
                ? Error.NotFound(description: "Material not found")
                : material;
        }
    }
}
