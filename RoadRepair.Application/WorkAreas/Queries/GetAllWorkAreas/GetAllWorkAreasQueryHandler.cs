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

namespace RoadRepair.Application.WorkAreas.Queries.GetAllWorkAreas
{
    public class GetAllWorkAreasQueryHandler : IRequestHandler<GetAllWorkAreasQuery, ErrorOr<List<WorkArea>>>
    {
        private IWorkAreaRepository _workAreaRepository;
        public GetAllWorkAreasQueryHandler(IWorkAreaRepository workAreaRepository)
        {
            _workAreaRepository = workAreaRepository;
        }

        public async Task<ErrorOr<List<WorkArea>>> Handle(GetAllWorkAreasQuery query, CancellationToken cancellationToken)
        {
            var workAreas = await _workAreaRepository.GetAllWorkAreasAsync();

            return workAreas is null
                ? Error.NotFound(description: "WorkArea not found")
                : workAreas.ToList();
        }
    }
}
