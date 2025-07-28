using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoadRepair.Application.MaterialSpends.Queries.GetAllMaterialSpends;
using RoadRepair.Application.RepairEventMedia.Commands.CreateRepairEventMedia;
using RoadRepair.Application.RepairEventMedia.Commands.DeleteRepairEventMedia;
using RoadRepair.Application.RepairEventMedia.Commands.UpdateRepairEventMedia;
using RoadRepair.Application.RepairEventMedia.Queries.GetAllRepairMedia;
using RoadRepair.Application.RepairEventMedia.Queries.GetRepairEventMedia;
using RoadRepair.Contracts.RepairEventMedia.CreateRepairEventMedia;
using RoadRepair.Contracts.RepairEventMedia.GetAllRepairEventMedia;
using RoadRepair.Contracts.RepairEventMedia.GetRepairEventMedia;
using RoadRepair.Contracts.RepairEventMedia.UpdateRepairEventMedia;
using RoadRepair.Domain.Entities;

namespace RoadRepair.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RepairEventMediaController : ControllerBase
    {
        private IMediator _mediator;
        public RepairEventMediaController(IMediator mediatr)
        {
            _mediator = mediatr;
        }

        [HttpGet("repairEvent/{repairEventId:long}")]
        public async Task<IActionResult> GetAllRepairEventMedia(long repairEventId)
        {
            var query = new GetAllRepairEventMediaQuery(repairEventId);

            var getAllRepairEventMediaResult = await _mediator.Send(query);

            return getAllRepairEventMediaResult.MatchFirst(
                materialSpend => Ok(new List<RepairEventMediaInfo>(materialSpend.Select(x => new RepairEventMediaInfo(x.Id, x.RepairEventId, x.FilePath, x.CreatedAt, x.Description)))),
                error => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "There are no RepairEventMedia with such repairEventId"));
        }

        [HttpGet("{repairEventMediaId:long}")]
        public async Task<IActionResult> GetRepairEventMedia(long repairEventMediaId)
        {
            var query = new GetRepairEventMediaQuery(repairEventMediaId);

            var getRepairEventMediaResult = await _mediator.Send(query);

            return getRepairEventMediaResult.MatchFirst(
                repairEventMedia => Ok(new GetRepairEventMediaResponse(repairEventMedia.Id, repairEventMedia.RepairEventId, repairEventMedia.FilePath, repairEventMedia.CreatedAt, repairEventMedia.Description)),
                error => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "There is no RepairEventMedia with such RepairEventMediaId"));
        }

        [HttpPost]
        public async Task<IActionResult> CreateRepairEventMedia([FromForm] CreateRepairEventMediaRequest request)
        {
            var command = new CreateRepairEventMediaCommand(request.RepairEventId, request.File, request.Description);

            var createRepairEventMediaResult = await _mediator.Send(command);

            return createRepairEventMediaResult.MatchFirst(
                repairEventMedia => Ok(new CreateRepairEventMediaResponse(repairEventMedia.Id)),
                error => Problem(error.Description));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRepairEventMedia(long id, [FromForm] UpdateRepairEventMediaRequest request)
        {
            var command = new UpdateRepairEventMediaCommand(id, request.RepairEventId, request.File, request.Description);

            var result = await _mediator.Send(command);

            return result.MatchFirst(
                _ => Ok(null),
                error => Problem(error.Description));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRepairEventMedia(long id)
        {
            var command = new DeleteRepairEventMediaCommand(id);

            var result = await _mediator.Send(command);

            return result.MatchFirst(
                _ => Ok(null),
                error => Problem(error.Description));
        }
    }
}
