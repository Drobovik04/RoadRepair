using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoadRepair.Application.MaterialSpends.Commands.CreateMaterialSpend;
using RoadRepair.Application.MaterialSpends.Commands.DeleteMaterialSpend;
using RoadRepair.Application.MaterialSpends.Commands.UpdateMaterialSpend;
using RoadRepair.Application.MaterialSpends.Queries.GetAllMaterialSpends;
using RoadRepair.Application.MaterialSpends.Queries.GetMaterialSpend;
using RoadRepair.Contracts.MaterialSpends.CreateMaterialSpend;
using RoadRepair.Contracts.MaterialSpends.GetAllMaterialSpends;
using RoadRepair.Contracts.MaterialSpends.GetMaterialSpend;
using RoadRepair.Contracts.MaterialSpends.UpdateMaterialSpend;


namespace RoadRepair.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaterialSpendController : ControllerBase
    {
        private IMediator _mediator;
        public MaterialSpendController(IMediator mediatr)
        {
            _mediator = mediatr;
        }

        [HttpGet("repairEvent/{repairEventId:long}")]
        public async Task<IActionResult> GetAllMaterialSpends(long repairEventId)
        {
            var query = new GetAllMaterialSpendsQuery(repairEventId);

            var getAllMaterialSpendsResult = await _mediator.Send(query);

            return getAllMaterialSpendsResult.MatchFirst(
                materialSpend => Ok(new List<MaterialSpendInfo>(materialSpend.Select(x => new MaterialSpendInfo(x.Id, x.MaterialId, x.Material.Name, x.Material.TypeOfMeasureId, x.Material.TypeOfMeasure.Name, x.Price, x.Volume, x.RepairEventId, x.ContractorId, x.Contractor.Name)))),
                error => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "There are no MaterialSpends with such repairEventId"));
        }

        [HttpGet("{materialSpendId:long}")]
        public async Task<IActionResult> GetMaterialSpend(long materialSpendId)
        {
            var query = new GetMaterialSpendQuery(materialSpendId);

            var getMaterialSpendResult = await _mediator.Send(query);

            return getMaterialSpendResult.MatchFirst(
                materialSpend => Ok(new GetMaterialSpendResponse(materialSpend.Id, materialSpend.MaterialId, materialSpend.Price, materialSpend.Volume, materialSpend.RepairEventId, materialSpend.ContractorId)),
                error => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "There is no MaterialSpend with such MaterialSpendId"));
        }

        [HttpPost]
        public async Task<IActionResult> CreateMaterialSpend([FromBody] CreateMaterialSpendRequest request)
        {
            var command = new CreateMaterialSpendCommand(request.MaterialId, request.Price, request.Volume, request.RepairEventId, request.ContractorId);

            var createMaterialSpendResult = await _mediator.Send(command);

            return createMaterialSpendResult.MatchFirst(
                materialSpend => Ok(new CreateMaterialSpendResponse(materialSpend.Id)),
                error => Problem(error.Description));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMaterialSpend(long id, [FromBody] UpdateMaterialSpendRequest request)
        {
            var command = new UpdateMaterialSpendCommand(id, request.MaterialId, request.Price, request.Volume, request.RepairEventId, request.ContractorId);

            var result = await _mediator.Send(command);

            return result.MatchFirst(
                _ => Ok(null),
                error => Problem(error.Description));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaterialSpend(long id)
        {
            var command = new DeleteMaterialSpendCommand(id);

            var result = await _mediator.Send(command);

            return result.MatchFirst(
                _ => Ok(null),
                error => Problem(error.Description));
        }
    }
}
