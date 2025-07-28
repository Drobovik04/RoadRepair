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

namespace RoadRepair.Application.WorkTimes.Queries.GetAllWorkTimesByRepairEventWorkerId
{
    public class GetAllWorkTimesByRepairEventWorkerIdQueryHandler : IRequestHandler<GetAllWorkTimesByRepairEventWorkerIdQuery, ErrorOr<List<WorkTime>>>
    {
        private IWorkTimeRepository _workTimeRepository;
        public GetAllWorkTimesByRepairEventWorkerIdQueryHandler(IWorkTimeRepository workTimeRepository)
        {
            _workTimeRepository = workTimeRepository;
        }

        public async Task<ErrorOr<List<WorkTime>>> Handle(GetAllWorkTimesByRepairEventWorkerIdQuery query, CancellationToken cancellationToken)
        {
            var workeTimes = await _workTimeRepository.GetAllWorkTimesByRepairEventWorkerIdAsync(query.RepairEventWorkerId);

            return workeTimes is null
                ? Error.NotFound(description: "WorkTimes are not found")
                : workeTimes.ToList();
        }
    }
}
