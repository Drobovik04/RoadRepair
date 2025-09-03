using ErrorOr;
using MediatR;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Domain.Entities;

namespace RoadRepair.Application.RepairEvents.Queries.GetRepairEvent
{
    public class GetRepairEventQueryHandler : IRequestHandler<GetRepairEventQuery, ErrorOr<RepairEvent>>
    {
        private IRepairEventRepository _repairEventsRepository;
        public GetRepairEventQueryHandler(IRepairEventRepository repairEventsRepository)
        {
            _repairEventsRepository = repairEventsRepository;
        }

        public async Task<ErrorOr<RepairEvent>> Handle(GetRepairEventQuery query, CancellationToken cancellationToken)
        {
            var repairEvent = await _repairEventsRepository.GetRepairEventByIdAsync(query.RepairEventId);

            return repairEvent is null
                ? Error.NotFound(description: "RepairEvent not found")
                : repairEvent;
        }
    }
}
