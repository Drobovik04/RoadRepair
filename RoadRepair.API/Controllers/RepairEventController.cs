using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoadRepair.Application.RepairEvents.Commands.CreateRepairEvent;
using RoadRepair.Application.RepairEvents.Commands.DeleteRepairEvent;
using RoadRepair.Application.RepairEvents.Commands.UpdateRepairEvent;
using RoadRepair.Application.RepairEvents.Queries.GetAllRepairEventsByRepairZoneId;
using RoadRepair.Application.RepairEvents.Queries.GetAllRepairEventsByWorkAreaId;
using RoadRepair.Application.RepairEvents.Queries.GetRepairEvent;
using RoadRepair.Contracts.RepairEvents.CreateRepairEvent;
using RoadRepair.Contracts.RepairEvents.GetAllRepairEvents;
using RoadRepair.Contracts.RepairEvents.GetRepairEvent;
using RoadRepair.Contracts.RepairEvents.UpdateRepairEvent;

namespace RoadRepair.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RepairEventController : ControllerBase
    {
        private IMediator _mediator;
        public RepairEventController(IMediator mediatr)
        {
            _mediator = mediatr;
        }

        [HttpGet("repairZone/{repairZoneId:long}")]
        public async Task<IActionResult> GetAllRepairEventsByRepairZoneId(long repairZoneId)
        {
            var query = new GetAllRepairEventsByRepairZoneIdQuery(repairZoneId);

            var getRepairEventsResult = await _mediator.Send(query);

            return getRepairEventsResult.MatchFirst(
                repairEvents => Ok(new List<RepairEventInfo>(repairEvents.Select(x => new RepairEventInfo(x.Id, x.RepairZoneId, x.StartedAt, x.EndedAt, x.TypeOfRepairId, x.TypeOfRepair.Name)))),
                error => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "There are no RepairEvents"));
        }

        [HttpGet("workArea/{workAreaId:long}")]
        public async Task<IActionResult> GetAllRepairEventsByWorkAreaId(long workAreaId)
        {
            var query = new GetAllRepairEventsByWorkAreaIdQuery(workAreaId);

            var getRepairEventsResult = await _mediator.Send(query);

            return getRepairEventsResult.MatchFirst(
                repairEvents => Ok(new List<RepairEventInfo>(repairEvents.Select(x => new RepairEventInfo(x.Id, x.RepairZoneId, x.StartedAt, x.EndedAt, x.TypeOfRepairId, x.TypeOfRepair.Name)))),
                error => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "There are no RepairEvents"));
        }

        [HttpGet("{repairEventId:long}")]
        public async Task<IActionResult> GetRepairEvent(long repairEventId)
        {
            var query = new GetRepairEventQuery(repairEventId);

            var getRepairEventResult = await _mediator.Send(query);

            return getRepairEventResult.MatchFirst(
                repairEvent => Ok(new GetRepairEventResponse(repairEvent.Id, repairEvent.RepairZoneId, repairEvent.StartedAt, repairEvent.EndedAt, repairEvent.TypeOfRepairId)),
                error => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "There is no RepairEvent with such RepairEventId"));
        }

        [HttpPost]
        public async Task<IActionResult> CreateRepairEvent([FromBody] CreateRepairEventRequest request)
        {
            var command = new CreateRepairEventCommand(request.RepairZoneId, request.StartedAt, request.EndedAt, request.TypeOfRepairId);

            var createRepairEventResult = await _mediator.Send(command);

            return createRepairEventResult.MatchFirst(
                repairEvent => Ok(new CreateRepairEventResponse(repairEvent.Id)),
                error => Problem(error.Description));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRepairEvent(long id, [FromBody] UpdateRepairEventRequest request)
        {
            var command = new UpdateRepairEventCommand(id, request.RepairZoneId, request.StartedAt, request.EndedAt, request.TypeOfRepairId);

            var result = await _mediator.Send(command);

            return result.MatchFirst(
                _ => Ok(null),
                error => Problem(error.Description));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRepairEvent(long id)
        {
            var command = new DeleteRepairEventCommand(id);

            var result = await _mediator.Send(command);

            return result.MatchFirst(
                _ => Ok(null),
                error => Problem(error.Description));
        }
    }
}
