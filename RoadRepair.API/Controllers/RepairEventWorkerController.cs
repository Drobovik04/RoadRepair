using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoadRepair.Application.Materials.Queries.GetAllMaterials;
using RoadRepair.Application.RepairEvents.Commands.CreateRepairEvent;
using RoadRepair.Application.RepairEventWorkers.Commands.CreateRepairEventWorker;
using RoadRepair.Application.RepairEventWorkers.Commands.DeleteRepairEventWorker;
using RoadRepair.Application.RepairEventWorkers.Commands.UpdateRepairEventWorker;
using RoadRepair.Application.RepairEventWorkers.Queries.GetAllRepairEventWorkersByRepairEventId;
using RoadRepair.Application.RepairEventWorkers.Queries.GetRepairEventWorker;
using RoadRepair.Contracts.RepairEventWorkers.CreateRepairEventWorker;
using RoadRepair.Contracts.RepairEventWorkers.GetAllRepairEventWorkers;
using RoadRepair.Contracts.RepairEventWorkers.GetRepairEventWorker;
using RoadRepair.Contracts.RepairEventWorkers.UpdateRepairEventWorker;

namespace RoadRepair.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RepairEventWorkerController : ControllerBase
    {
        private IMediator _mediator;
        public RepairEventWorkerController(IMediator mediatr)
        {
            _mediator = mediatr;
        }

        [HttpGet("repairEvent/{repairEventId:long}")]
        public async Task<IActionResult> GetAllRepairEventWorkers(long repairEventId)
        {
            var query = new GetAllRepairEventWorkersByRepairEventIdQuery(repairEventId);

            var getResult = await _mediator.Send(query);

            return getResult.MatchFirst(
                res => Ok(new List<RepairEventWorkerInfo>(res.Select(x => new RepairEventWorkerInfo(x.Id, x.RepairEventId, x.WorkerId)).ToList())),
                error => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "There are no RepairEventWorkers"));
        }

        [HttpGet("{repairEventWorkerId:long}")]
        public async Task<IActionResult> GetRepairEventWorker(long repairEventWorkerId)
        {
            var query = new GetRepairEventWorkerQuery(repairEventWorkerId);

            var getRepairEventWorkerResult = await _mediator.Send(query);

            return getRepairEventWorkerResult.MatchFirst(
                repairEventWorker => Ok(new GetRepairEventWorkerResponse(repairEventWorker.Id, repairEventWorker.RepairEventId, repairEventWorker.WorkerId)),
                error => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "There is no RepairEventWorker with such RepairEventWorkerId"));
        }

        [HttpPost]
        public async Task<IActionResult> CreateRepairEventWorker([FromBody] CreateRepairEventWorkerRequest request)
        {
            var command = new CreateRepairEventWorkerCommand(request.RepairEventId, request.WorkerId);

            var createRepairEventWorkerResult = await _mediator.Send(command);

            return createRepairEventWorkerResult.MatchFirst(
                repairEventWorker => Ok(new CreateRepairEventWorkerResponse(repairEventWorker.Id)),
                error => Problem(error.Description));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRepairEventWorker(long id, [FromBody] UpdateRepairEventWorkerRequest request)
        {
            var command = new UpdateRepairEventWorkerCommand(id, request.RepairEventId, request.WorkerId);

            var result = await _mediator.Send(command);

            return result.MatchFirst(
                _ => Ok(null),
                error => Problem(error.Description));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRepairEventWorker(long id)
        {
            var command = new DeleteRepairEventWorkerCommand(id);

            var result = await _mediator.Send(command);

            return result.MatchFirst(
                _ => Ok(null),
                error => Problem(error.Description));
        }
    }
}
