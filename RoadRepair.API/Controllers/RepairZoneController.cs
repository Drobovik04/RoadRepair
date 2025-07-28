using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoadRepair.Application.RepairZones.Commands.CreateRepairZone;
using RoadRepair.Application.RepairZones.Commands.DeleteRepairZone;
using RoadRepair.Application.RepairZones.Commands.UpdateRepairZone;
using RoadRepair.Application.RepairZones.Queries.GetAllRepairZones;
using RoadRepair.Application.RepairZones.Queries.GetRepairZone;
using RoadRepair.Application.WorkAreas.Queries.GetAllWorkAreas;
using RoadRepair.Contracts.RepairZones.CreateRepairZone;
using RoadRepair.Contracts.RepairZones.GetAllRepairZones;
using RoadRepair.Contracts.RepairZones.GetRepairZone;
using RoadRepair.Contracts.RepairZones.UpdateRepairZone;


namespace RoadRepair.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RepairZoneController : ControllerBase
    {
        private IMediator _mediator;
        public RepairZoneController(IMediator mediatr)
        {
            _mediator = mediatr;
        }

        [HttpGet("workArea/{workAreaId:long}")]
        public async Task<IActionResult> GetAllRepairZones(long workAreaId)
        {
            var query = new GetAllRepairZonesQuery(workAreaId);

            var getRepairZonesResult = await _mediator.Send(query);

            return getRepairZonesResult.MatchFirst(
                repairZones => Ok(new List<RepairZoneInfo>(repairZones.Select(x => new RepairZoneInfo(x.Id, x.WorkAreaId, x.GeometryJson, x.Name)))),
                error => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "There are no RepairAreas"));
        }

        [HttpGet("{repairZoneId:long}")]
        public async Task<IActionResult> GetRepairZone(long repairZoneId)
        {
            var query = new GetRepairZoneQuery(repairZoneId);

            var getRepairZoneResult = await _mediator.Send(query);

            return getRepairZoneResult.MatchFirst(
                repairZone => Ok(new GetRepairZoneResponse(repairZone.Id, repairZone.WorkAreaId, repairZone.GeometryJson, repairZone.Name)),
                error => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "There is no RepairZone with such RepairZoneId"));
        }

        [HttpPost]
        public async Task<IActionResult> CreateRepairZone([FromBody] CreateRepairZoneRequest request)
        {
            var command = new CreateRepairZoneCommand(request.WorkAreaId, request.GeometryJson, request.Name);

            var createRepairZoneResult = await _mediator.Send(command);

            return createRepairZoneResult.MatchFirst(
                repairZone => Ok(new CreateRepairZoneResponse(repairZone.Id)),
                error => Problem(error.Description));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRepairZone(long id, [FromBody] UpdateRepairZoneRequest request)
        {
            var command = new UpdateRepairZoneCommand(id, request.WorkAreaId, request.GeometryJson, request.Name);

            var result = await _mediator.Send(command);

            return result.MatchFirst(
                _ => Ok(null),
                error => Problem(error.Description));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRepairZone(long id)
        {
            var command = new DeleteRepairZoneCommand(id);

            var result = await _mediator.Send(command);

            return result.MatchFirst(
                _ => Ok(null),
                error => Problem(error.Description));
        }
    }
}
