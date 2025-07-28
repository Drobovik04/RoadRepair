using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoadRepair.Application.TypesOfService.Commands.CreateTypeOfService;
using RoadRepair.Application.TypesOfService.Commands.DeleteTypeOfService;
using RoadRepair.Application.TypesOfService.Commands.UpdateTypeOfService;
using RoadRepair.Application.TypesOfService.Queries.GetAllTypesOfService;
using RoadRepair.Application.TypesOfService.Queries.GetTypeOfService;
using RoadRepair.Contracts.TypesOfService.CreateTypeOfService;
using RoadRepair.Contracts.TypesOfService.GetAllTypesOfService;
using RoadRepair.Contracts.TypesOfService.GetTypeOfService;
using RoadRepair.Contracts.TypesOfService.UpdateTypeOfService;
using RoadRepair.Domain.Entities;

namespace RoadRepair.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TypeOfServiceController : ControllerBase
    {
        private IMediator _mediator;
        public TypeOfServiceController(IMediator mediatr)
        {
            _mediator = mediatr;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTypesOfService()
        {
            var query = new GetAllTypesOfServiceQuery();

            var getAllTypesOfServiceResult = await _mediator.Send(query);

            return getAllTypesOfServiceResult.MatchFirst(
                typesOfService => Ok(new List<TypeOfServiceInfo>(typesOfService.Select(x => new TypeOfServiceInfo(x.Id, x.Name)))),
                error => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "There are no TypesOfService"));
        }

        [HttpGet("{typeOfServiceId:long}")]
        public async Task<IActionResult> GetTypeOfService(long typeOfServiceId)
        {
            var query = new GetTypeOfServiceQuery(typeOfServiceId);

            var getTypeOfServiceResult = await _mediator.Send(query);

            return getTypeOfServiceResult.MatchFirst(
                typeOfService => Ok(new GetTypeOfServiceResponse(typeOfService.Id, typeOfService.Name)),
                error => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "There is no TypeOfService with such TypeOfServiceId"));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTypeOfService([FromBody] CreateTypeOfServiceRequest request)
        {
            var command = new CreateTypeOfServiceCommand(request.Name);

            var createTypeOfServiceResult = await _mediator.Send(command);

            return createTypeOfServiceResult.MatchFirst(
                typeOfService => Ok(new CreateTypeOfServiceResponse(typeOfService.Id)),
                error => Problem(error.Description));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTypeOfService(long id, [FromBody] UpdateTypeOfServiceRequest request)
        {
            var command = new UpdateTypeOfServiceCommand(id, request.Name);

            var result = await _mediator.Send(command);

            return result.MatchFirst(
                _ => Ok(null),
                error => Problem(error.Description));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTypeOfService(long id)
        {
            var command = new DeleteTypeOfServiceCommand(id);

            var result = await _mediator.Send(command);

            return result.MatchFirst(
                _ => Ok(null),
                error => Problem(error.Description));
        }
    }
}
