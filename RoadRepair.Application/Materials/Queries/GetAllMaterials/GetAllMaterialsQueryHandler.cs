using ErrorOr;
using MediatR;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Application.Organizations.Queries.GetOrganization;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Materials.Queries.GetAllMaterials
{
    public class GetAllMaterialsQueryHandler : IRequestHandler<GetAllMaterialsQuery, ErrorOr<List<Material>>>
    {
        private IMaterialRepository _materialRepository;
        public GetAllMaterialsQueryHandler(IMaterialRepository materialRepository)
        {
            _materialRepository = materialRepository;
        }

        public async Task<ErrorOr<List<Material>>> Handle(GetAllMaterialsQuery query, CancellationToken cancellationToken)
        {
            var materials = await _materialRepository.GetAllMaterialsAsync(query.organizationId);

            return materials is null
                ? Error.NotFound(description: "Material not found")
                : materials.ToList();
        }
    }
}
