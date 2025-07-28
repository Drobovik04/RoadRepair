using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoadRepair.Application.TypesOfRepair.Commands.CreateTypeOfRepair;
using RoadRepair.Application.TypesOfRepair.Commands.DeleteTypeOfRepair;
using RoadRepair.Application.TypesOfRepair.Commands.UpdateTypeOfRepair;
using RoadRepair.Application.TypesOfRepair.Queries.GetAllTypesOfRepair;
using RoadRepair.Application.TypesOfRepair.Queries.GetTypeOfRepair;
using RoadRepair.Application.TypesOfService.Queries.GetAllTypesOfService;
using RoadRepair.Contracts.TypesOfRepair.CreateTypeOfRepair;
using RoadRepair.Contracts.TypesOfRepair.GetAllTypesOfRepair;
using RoadRepair.Contracts.TypesOfRepair.GetTypeOfRepair;
using RoadRepair.Contracts.TypesOfRepair.UpdateTypeOfRepair;
using RoadRepair.Contracts.WorkTimes.CreateWorkTime;

namespace RoadRepair.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TypeOfRepairController : ControllerBase
    {
        private IMediator _mediator;
        public TypeOfRepairController(IMediator mediatr)
        {
            _mediator = mediatr;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTypesOfRepair()
        {
            var query = new GetAllTypesOfRepairQuery();

            var getAllTypesOfRepairResult = await _mediator.Send(query);

            return getAllTypesOfRepairResult.MatchFirst(
                typesOfRepair => Ok(new List<TypeOfRepairInfo>(typesOfRepair.Select(x => new TypeOfRepairInfo(x.Id, x.Name)))),
                error => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "There are no TypesOfRepair"));
        }

        [HttpGet("{typeOfRepairId:long}")]
        public async Task<IActionResult> GetTypeOfRepair(long typeOfRepairId)
        {
            var query = new GetTypeOfRepairQuery(typeOfRepairId);

            var getTypeOfRepairResult = await _mediator.Send(query);

            return getTypeOfRepairResult.MatchFirst(
                typeOfRepair => Ok(new GetTypeOfRepairResponse(typeOfRepair.Id, typeOfRepair.Name)),
                error => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "There is no TypeOfRepair with such TypeOfRepairId"));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTypeOfRepair([FromBody] CreateTypeOfRepairRequest request)
        {
            var command = new CreateTypeOfRepairCommand(request.Name);

            var createTypeOfRepairResult = await _mediator.Send(command);

            return createTypeOfRepairResult.MatchFirst(
                typeOfRepair => Ok(new CreateTypeOfRepairResponse(typeOfRepair.Id)),
                error => Problem(error.Description));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTypeOfRepair(long id, [FromBody] UpdateTypeOfRepairRequest request)
        {
            var command = new UpdateTypeOfRepairCommand(id, request.Name);

            var result = await _mediator.Send(command);

            return result.MatchFirst(
                _ => Ok(null),
                error => Problem(error.Description));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTypeOfRepair(long id)
        {
            var command = new DeleteTypeOfRepairCommand(id);

            var result = await _mediator.Send(command);

            return result.MatchFirst(
                _ => Ok(null),
                error => Problem(error.Description));
        }
    }
}
