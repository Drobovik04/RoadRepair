using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoadRepair.Application.Materials.Queries.GetAllMaterials;
using RoadRepair.Application.RepairEvents.Commands.CreateRepairEvent;
using RoadRepair.Application.WorkAreaWorkers.Commands.CreateWorkAreaWorker;
using RoadRepair.Application.WorkAreaWorkers.Commands.DeleteWorkAreaWorker;
using RoadRepair.Application.WorkAreaWorkers.Commands.UpdateWorkAreaWorker;
using RoadRepair.Application.WorkAreaWorkers.Queries.GetAllWorkAreaWorkersByWorkAreaId;
using RoadRepair.Application.WorkAreaWorkers.Queries.GetWorkAreaWorker;
using RoadRepair.Contracts.WorkAreaWorkers.CreateWorkAreaWorker;
using RoadRepair.Contracts.WorkAreaWorkers.GetAllWorkAreaWorkers;
using RoadRepair.Contracts.WorkAreaWorkers.GetWorkAreaWorker;
using RoadRepair.Contracts.WorkAreaWorkers.UpdateWorkAreaWorker;
using RoadRepair.Domain.Entities;

namespace RoadRepair.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkAreaWorkerController : ControllerBase
    {
        private IMediator _mediator;
        public WorkAreaWorkerController(IMediator mediatr)
        {
            _mediator = mediatr;
        }

        [HttpGet("workArea/{workAreaId:long}")]
        public async Task<IActionResult> GetAllWorkAreaWorkers(long workAreaId)
        {
            var query = new GetAllWorkAreaWorkersByWorkAreaIdQuery(workAreaId);

            var getResult = await _mediator.Send(query);

            return getResult.MatchFirst(
                res => Ok(new List<WorkAreaWorkerInfo>(res.Select(x => new WorkAreaWorkerInfo(x.Id, x.WorkAreaId, x.WorkerId)).ToList())),
                error => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "There are no WorkAreaWorkers"));
        }

        [HttpGet("{workAreaWorkerId:long}")]
        public async Task<IActionResult> GetWorkAreaWorker(long workAreaWorkerId)
        {
            var query = new GetWorkAreaWorkerQuery(workAreaWorkerId);

            var getWorkAreaWorkerResult = await _mediator.Send(query);

            return getWorkAreaWorkerResult.MatchFirst(
                workAreaWorker => Ok(new GetWorkAreaWorkerResponse(workAreaWorker.Id, workAreaWorker.WorkAreaId, workAreaWorker.WorkerId)),
                error => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "There is no WorkAreaWorker with such WorkAreaWorkerId"));
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorkAreaWorker([FromBody] CreateWorkAreaWorkerRequest request)
        {
            var command = new CreateWorkAreaWorkerCommand(request.WorkAreaId, request.WorkerId);

            var createWorkAreaWorkerResult = await _mediator.Send(command);

            return createWorkAreaWorkerResult.MatchFirst(
                workAreaWorker => Ok(new CreateWorkAreaWorkerResponse(workAreaWorker.Id)),
                error => Problem(error.Description));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorkAreaWorker(long id, [FromBody] UpdateWorkAreaWorkerRequest request)
        {
            var command = new UpdateWorkAreaWorkerCommand(id, request.WorkAreaId, request.WorkerId);

            var result = await _mediator.Send(command);

            return result.MatchFirst(
                _ => Ok(null),
                error => Problem(error.Description));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkAreaWorker(long id)
        {
            var command = new DeleteWorkAreaWorkerCommand(id);

            var result = await _mediator.Send(command);

            return result.MatchFirst(
                _ => Ok(null),
                error => Problem(error.Description));
        }
    }
}
