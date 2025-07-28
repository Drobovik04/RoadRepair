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

namespace RoadRepair.Application.WorkTimes.Queries.GetWorkTime
{
    public class GetWorkTimeQueryHandler : IRequestHandler<GetWorkTimeQuery, ErrorOr<WorkTime>>
    {
        private IWorkTimeRepository _workTimeRepository;
        public GetWorkTimeQueryHandler(IWorkTimeRepository workTimeRepository)
        {
            _workTimeRepository = workTimeRepository;
        }

        public async Task<ErrorOr<WorkTime>> Handle(GetWorkTimeQuery query, CancellationToken cancellationToken)
        {
            var workTime = await _workTimeRepository.GetWorkTimeByIdAsync(query.WorkTimeId);

            return workTime is null
                ? Error.NotFound(description: "WorkTime not found")
                : workTime;
        }
    }
}
