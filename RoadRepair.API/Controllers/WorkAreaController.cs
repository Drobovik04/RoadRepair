using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoadRepair.Application.WorkAreas.Commands.CreateWorkArea;
using RoadRepair.Application.WorkAreas.Commands.DeleteWorkArea;
using RoadRepair.Application.WorkAreas.Commands.UpdateWorkArea;
using RoadRepair.Application.WorkAreas.Queries.GetAllWorkAreas;
using RoadRepair.Application.WorkAreas.Queries.GetWorkArea;
using RoadRepair.Application.Workers.Queries.GetAllWorkers;
using RoadRepair.Application.WorkTimes.Commands.CreateWorkTime;
using RoadRepair.Application.WorkTimes.Commands.DeleteWorkTime;
using RoadRepair.Application.WorkTimes.Commands.UpdateWorkTime;
using RoadRepair.Application.WorkTimes.Queries.GetWorkTime;
using RoadRepair.Contracts.WorkAreas.CreateWorkArea;
using RoadRepair.Contracts.WorkAreas.GetAllWorkAreas;
using RoadRepair.Contracts.WorkAreas.GetWorkArea;
using RoadRepair.Contracts.WorkAreas.UpdateWorkArea;
using RoadRepair.Contracts.WorkTimes.CreateWorkTime;
using RoadRepair.Contracts.WorkTimes.GetWorkTime;
using RoadRepair.Contracts.WorkTimes.UpdateWorkTime;
using RoadRepair.Domain.Entities;

namespace RoadRepair.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkAreaController : ControllerBase
    {
        private IMediator _mediator;
        public WorkAreaController(IMediator mediatr)
        {
            _mediator = mediatr;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWorkAreas()
        {
            var query = new GetAllWorkAreasQuery();

            var getWorkAreasResult = await _mediator.Send(query);

            return getWorkAreasResult.MatchFirst(
                workAreas => Ok(new List<WorkAreaInfo>(workAreas.Select(x => new WorkAreaInfo(x.Id, x.OrganizationId, x.Name, x.Description, x.CreatedAt, x.UpdatedAt, x.ResponsibleId, x.Responsible?.LastName, x.Responsible?.FirstName, x.Responsible?.MiddleName)))),
                error => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "There are no WorkAreas"));
        }

        [HttpGet("{workAreaId:long}")]
        public async Task<IActionResult> GetWorkArea(long workAreaId)
        {
            var query = new GetWorkAreaQuery(workAreaId);

            var getWorkAreaResult = await _mediator.Send(query);

            return getWorkAreaResult.MatchFirst(
                workArea => Ok(new GetWorkAreaResponse(workArea.Id, workArea.OrganizationId, workArea.Name, workArea.Description, workArea.CreatedAt, workArea.UpdatedAt, workArea.ResponsibleId, workArea.Responsible?.LastName, workArea.Responsible?.FirstName, workArea.Responsible?.MiddleName)),
                error => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "There is no WorkArea with such WorkAreaId"));
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorkArea([FromBody] CreateWorkAreaRequest request)
        {
            var command = new CreateWorkAreaCommand(request.OrganizationId, request.Name, request.Description, request.CreatedAt, request.UpdatedAt, request.ResponsibleId);

            var createWorkAreaResult = await _mediator.Send(command);

            return createWorkAreaResult.MatchFirst(
                workArea => Ok(new CreateWorkAreaResponse(workArea.Id)),
                error => Problem(error.Description));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorkArea(long id, [FromBody] UpdateWorkAreaRequest request)
        {
            var command = new UpdateWorkAreaCommand(id, request.OrganizationId, request.Name, request.Description, request.CreatedAt, request.UpdatedAt, request.ResponsibleId);

            var result = await _mediator.Send(command);

            return result.MatchFirst(
                _ => Ok(null),
                error => Problem(error.Description));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkArea(long id)
        {
            var command = new DeleteWorkAreaCommand(id);

            var result = await _mediator.Send(command);

            return result.MatchFirst(
                _ => Ok(null),
                error => Problem(error.Description));
        }
    }
}
