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

namespace RoadRepair.Application.WorkTimes.Queries.GetAllWorkTimesByWorkAreaWorkerId
{
    public class GetAllWorkTimesByWorkAreaWorkerIdQueryHandler : IRequestHandler<GetAllWorkTimesByWorkAreaWorkerIdQuery, ErrorOr<List<WorkTime>>>
    {
        private IWorkTimeRepository _workTimeRepository;
        public GetAllWorkTimesByWorkAreaWorkerIdQueryHandler(IWorkTimeRepository workTimeRepository)
        {
            _workTimeRepository = workTimeRepository;
        }

        public async Task<ErrorOr<List<WorkTime>>> Handle(GetAllWorkTimesByWorkAreaWorkerIdQuery query, CancellationToken cancellationToken)
        {
            var workeTimes = await _workTimeRepository.GetAllWorkTimesByWorkAreaWorkerIdAsync(query.WorkAreaWorkerId);

            return workeTimes is null
                ? Error.NotFound(description: "WorkTimes are not found")
                : workeTimes.ToList();
        }
    }
}
