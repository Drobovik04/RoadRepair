using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoadRepair.Application.Workers.Queries.GetAllWorkers;
using RoadRepair.Application.WorkTimes.Commands.BulkUpdateWorkTime;
using RoadRepair.Application.WorkTimes.Commands.CreateWorkTime;
using RoadRepair.Application.WorkTimes.Commands.DeleteAllWorkTimesByWorkAreaWorkerId;
using RoadRepair.Application.WorkTimes.Commands.DeleteWorkTime;
using RoadRepair.Application.WorkTimes.Commands.UpdateWorkTime;
using RoadRepair.Application.WorkTimes.Queries.GetAllWorkTimesByWorkAreaWorkerId;
using RoadRepair.Application.WorkTimes.Queries.GetWorkTime;
using RoadRepair.Contracts.WorkTimes.BulkUpdateWorkTime;
using RoadRepair.Contracts.WorkTimes.CreateWorkTime;
using RoadRepair.Contracts.WorkTimes.GetWorkTime;
using RoadRepair.Contracts.WorkTimes.UpdateWorkTime;
using RoadRepair.Domain.Entities;
using WorkTimeInfo = RoadRepair.Contracts.WorkTimes.BulkUpdateWorkTime.WorkTimeInfo;

namespace RoadRepair.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkTimeController : ControllerBase
    {
        private IMediator _mediator;
        public WorkTimeController(IMediator mediatr)
        {
            _mediator = mediatr;
        }

        [HttpGet("workAreaWorker/{workAreaWorkerId:long}")]
        public async Task<IActionResult> GetAllWorkTimesByWorkAreaWorkerId(long workAreaWorkerId)
        {
            var query = new GetAllWorkTimesByWorkAreaWorkerIdQuery(workAreaWorkerId);

            var getWorkTimesResult = await _mediator.Send(query);

            return getWorkTimesResult.MatchFirst(
                workTime => Ok(new List<Contracts.WorkTimes.GetAllWorkTimesByWorkAreaWorkerId.WorkTimeInfo>(workTime.Select(x => new Contracts.WorkTimes.GetAllWorkTimesByWorkAreaWorkerId.WorkTimeInfo(x.Id, x.DayOfWork, (float)x.Hours, x.WorkAreaWorkerId)))),
                error => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "There are no WorkTimes with such WorkAreaWorkerId"));
        }

        [HttpGet("{workTimeId:long}")]
        public async Task<IActionResult> GetWorkTime(long workTimeId)
        {
            var query = new GetWorkTimeQuery(workTimeId);

            var getWorkTimeResult = await _mediator.Send(query);

            return getWorkTimeResult.MatchFirst(
                workTime => Ok(new GetWorkTimeResponse(workTime.Id, workTime.DayOfWork, workTime.Hours)),
                error => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "There is no WorkTime with such WorkTimeId"));
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorkTime([FromBody] CreateWorkTimeRequest request)
        {
            var command = new CreateWorkTimeCommand(request.DayOfWork, request.Hours, request.RepairEventWorkerId);

            var createWorkTimeResult = await _mediator.Send(command);

            return createWorkTimeResult.MatchFirst(
                workTime => Ok(new CreateWorkTimeResponse(workTime.Id)),
                error => Problem(error.Description));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorkTime(long id, [FromBody] UpdateWorkTimeRequest request)
        {
            var command = new UpdateWorkTimeCommand(id, request.DayOfWork, request.Hours, request.RepairEventWorkerId);

            var result = await _mediator.Send(command);

            return result.MatchFirst(
                _ => Ok(null),
                error => Problem(error.Description));
        }

        [HttpPost("bulkWorkTime/")]
        public async Task<IActionResult> BulkUpdateWorkTime([FromBody] List<WorkTimeInfo> request)
        {
            var command = new BulkUpdateWorkTimeCommand(request.Select(x => new WorkTime {Id = x.Id, DayOfWork = x.DayOfWork, Hours = x.Hours, WorkAreaWorkerId = x.WorkAreaWorkerId }).ToList());

            var result = await _mediator.Send(command);

            return result.MatchFirst(
                _ => Ok(null),
                error => Problem(error.Description));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkTime(long id)
        {
            var command = new DeleteWorkTimeCommand(id);

            var result = await _mediator.Send(command);

            return result.MatchFirst(
                _ => Ok(null),
                error => Problem(error.Description));
        }

        [HttpDelete("workAreaWorker/{workAreaWorkerId}")]
        public async Task<IActionResult> DeleteAllWorkTimesByWorkAreaWorkerId(long workAreaWorkerId)
        {
            var command = new DeleteAllWorkTimesByWorkAreaWorkerIdCommand(workAreaWorkerId);

            var result = await _mediator.Send(command);

            return result.MatchFirst(
                _ => Ok(null),
                error => Problem(error.Description));
        }
    }
}
