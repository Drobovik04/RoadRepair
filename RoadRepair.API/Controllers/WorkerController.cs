using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoadRepair.Application.Workers.Commands.CreateWorker;
using RoadRepair.Application.Workers.Commands.DeleteWorker;
using RoadRepair.Application.Workers.Commands.UpdateWorker;
using RoadRepair.Application.Workers.Queries.GetAllWorkers;
using RoadRepair.Application.Workers.Queries.GetWorker;
using RoadRepair.Contracts.Workers.CreateWorker;
using RoadRepair.Contracts.Workers.GetAllWorkers;
using RoadRepair.Contracts.Workers.GetWorker;
using RoadRepair.Contracts.Workers.UpdateWorker;


namespace RoadRepair.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkerController : ControllerBase
    {
        private IMediator _mediator;
        public WorkerController(IMediator mediatr)
        {
            _mediator = mediatr;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWorkers()
        {
            var query = new GetAllWorkersQuery();

            var getWorkersResult = await _mediator.Send(query);

            return getWorkersResult.MatchFirst(
                workers => Ok(new List<WorkerInfo>(workers.Select(x => new WorkerInfo(x.Id, x.OrganizationId, x.LastName, x.MiddleName, x.FirstName, x.HiredAt, x.FiredAt, x.PositionId, x.Position?.Name)))),
                error => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "There are no Workers"));
        }
        [HttpGet("{workerId:long}")]
        public async Task<IActionResult> GetWorker(long workerId)
        {
            var query = new GetWorkerQuery(workerId);

            var getWorkerResult = await _mediator.Send(query);

            return getWorkerResult.MatchFirst(
                worker => Ok(new GetWorkerResponse(worker.Id, worker.OrganizationId, worker.LastName, worker.MiddleName, worker.FirstName, worker.HiredAt, worker.FiredAt, worker.OrganizationId)),
                error => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "There is no Worker with such WorkerId"));
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorker([FromBody] CreateWorkerRequest request)
        {
            var command = new CreateWorkerCommand(request.OrganizationId, request.LastName, request.MiddleName, request.FirstName, request.HiredAt, request.PositionId);

            var createWorkerResult = await _mediator.Send(command);

            return createWorkerResult.MatchFirst(
                worker => Ok(new CreateWorkerResponse(worker.Id)),
                error => Problem(error.Description));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorker(long id, [FromBody] UpdateWorkerRequest request)
        {
            var command = new UpdateWorkerCommand(id, request.OrganizationId, request.LastName, request.MiddleName, request.FirstName, request.HiredAt, request.FiredAt, request.PositionId);

            var result = await _mediator.Send(command);

            return result.MatchFirst(
                _ => Ok(null),
                error => Problem(error.Description));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorker(long id)
        {
            var command = new DeleteWorkerCommand(id);

            var result = await _mediator.Send(command);

            return result.MatchFirst(
                _ => Ok(null),
                error => Problem(error.Description));
        }
    }
}
