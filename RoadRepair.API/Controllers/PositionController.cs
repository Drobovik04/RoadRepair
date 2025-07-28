using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoadRepair.Application.Contractors.Queries.GetAllContractors;
using RoadRepair.Application.Positions.Commands.CreatePosition;
using RoadRepair.Application.Positions.Commands.DeletePosition;
using RoadRepair.Application.Positions.Commands.UpdatePosition;
using RoadRepair.Application.Positions.Queries.GetAllPositions;
using RoadRepair.Application.Positions.Queries.GetPosition;
using RoadRepair.Contracts.Positions.CreatePosition;
using RoadRepair.Contracts.Positions.GetAllPositions;
using RoadRepair.Contracts.Positions.GetPosition;
using RoadRepair.Contracts.Positions.UpdatePosition;

namespace RoadRepair.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PositionController : ControllerBase
    {
        private IMediator _mediator;
        public PositionController(IMediator mediatr)
        {
            _mediator = mediatr;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPositions()
        {
            var query = new GetAllPositionsQuery();

            var getPositionsResult = await _mediator.Send(query);

            return getPositionsResult.MatchFirst(
                positions => Ok(new List<PositionInfo>(positions.Select(x => new PositionInfo(x.Id, x.Name)).ToList())),
                error => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "There are no Positions"));
        }
        [HttpGet("{positionId:long}")]
        public async Task<IActionResult> GetPosition(long positionId)
        {
            var query = new GetPositionQuery(positionId);

            var getPositionResult = await _mediator.Send(query);

            return getPositionResult.MatchFirst(
                position => Ok(new GetPositionResponse(position.Id, position.Name)),
                error => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "There is no Position with such PositionId"));
        }

        [HttpPost]
        public async Task<IActionResult> CreatePosition([FromBody] CreatePositionRequest request)
        {
            var command = new CreatePositionCommand(request.Name);

            var createPositionResult = await _mediator.Send(command);

            return createPositionResult.MatchFirst(
                position => Ok(new CreatePositionResponse(position.Id)),
                error => Problem(error.Description));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePosition(long id, [FromBody] UpdatePositionRequest request)
        {
            var command = new UpdatePositionCommand(id, request.Name);

            var result = await _mediator.Send(command);

            return result.MatchFirst(
                _ => Ok(null),
                error => Problem(error.Description));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePosition(long id)
        {
            var command = new DeletePositionCommand(id);

            var result = await _mediator.Send(command);

            return result.MatchFirst(
                _ => Ok(null),
                error => Problem(error.Description));
        }
    }
}
