using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoadRepair.Application.TypesOfMeasure.Commands.CreateTypeOfMeasure;
using RoadRepair.Application.TypesOfMeasure.Commands.DeleteTypeOfMeasure;
using RoadRepair.Application.TypesOfMeasure.Commands.UpdateTypeOfMeasure;
using RoadRepair.Application.TypesOfMeasure.Queries.GetAllTypesOfMeasure;
using RoadRepair.Application.TypesOfMeasure.Queries.GetTypeOfMeasure;
using RoadRepair.Contracts.TypesOfMeasure.CreateTypeOfMeasure;
using RoadRepair.Contracts.TypesOfMeasure.GetAllTypesOfMeasure;
using RoadRepair.Contracts.TypesOfMeasure.GetTypeOfMeasure;
using RoadRepair.Contracts.TypesOfMeasure.UpdateTypeOfMeasure;

namespace RoadRepair.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TypeOfMeasureController : ControllerBase
    {
        private IMediator _mediator;
        public TypeOfMeasureController(IMediator mediatr)
        {
            _mediator = mediatr;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTypesOfMeasure()
        {
            var query = new GetAllTypesOfMeasureQuery();

            var getAllTypesOfMeasureResult = await _mediator.Send(query);

            return getAllTypesOfMeasureResult.MatchFirst(
                typesOfMeasure => Ok(new List<TypeOfMaterialInfo>(getAllTypesOfMeasureResult.Value.Select(x => new TypeOfMaterialInfo(x.Id, x.Name)).ToList())),
                error => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "There are no TypesOfMeasure"));
        }

        [HttpGet("{typeOfMeasureId:long}")]
        public async Task<IActionResult> GetTypeOfMeasure(long typeOfMeasureId)
        {
            var query = new GetTypeOfMeasureQuery(typeOfMeasureId);

            var getTypeOfMeasureResult = await _mediator.Send(query);

            return getTypeOfMeasureResult.MatchFirst(
                typeOfMeasure => Ok(new GetTypeOfMeasureResponse(typeOfMeasure.Id, typeOfMeasure.Name)),
                error => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "There is no TypeOfMeasure with such TypeOfMeasureId"));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTypeOfMeasure([FromBody] CreateTypeOfMeasureRequest request)
        {
            var command = new CreateTypeOfMeasureCommand(request.Name);

            var createTypeOfMeasureResult = await _mediator.Send(command);

            return createTypeOfMeasureResult.MatchFirst(
                typeOfMeasure => Ok(new CreateTypeOfMeasureResponse(typeOfMeasure.Id)),
                error => Problem(error.Description));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTypeOfMeasure(long id, [FromBody] UpdateTypeOfMeasureRequest request)
        {
            var command = new UpdateTypeOfMeasureCommand(id, request.Name);

            var result = await _mediator.Send(command);

            return result.MatchFirst(
                _ => Ok(null),
                error => Problem(error.Description));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTypeOfMeasure(long id)
        {
            var command = new DeleteTypeOfMeasureCommand(id);

            var result = await _mediator.Send(command);

            return result.MatchFirst(
                _ => Ok(null),
                error => Problem(error.Description));
        }
    }
}
