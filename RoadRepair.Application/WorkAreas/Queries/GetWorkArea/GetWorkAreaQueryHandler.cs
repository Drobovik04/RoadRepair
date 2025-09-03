using ErrorOr;
using MediatR;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.WorkAreas.Queries.GetWorkArea
{
    public class GetWorkAreaQueryHandler : IRequestHandler<GetWorkAreaQuery, ErrorOr<WorkArea>>
    {
        private IWorkAreaRepository _workAreaRepository;
        public GetWorkAreaQueryHandler(IWorkAreaRepository workAreaRepository)
        {
            _workAreaRepository = workAreaRepository;
        }

        public async Task<ErrorOr<WorkArea>> Handle(GetWorkAreaQuery query, CancellationToken cancellationToken)
        {
            var workArea = await _workAreaRepository.GetWorkAreaByIdAsync(query.WorkAreaId);

            return workArea is null
                ? Error.NotFound(description: "WorkArea not found")
                : workArea;
        }
    }
}
