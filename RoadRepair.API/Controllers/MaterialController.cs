using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoadRepair.Application.Materials.Commands.CreateMaterial;
using RoadRepair.Application.Materials.Commands.DeleteMaterial;
using RoadRepair.Application.Materials.Commands.UpdateMaterial;
using RoadRepair.Application.Materials.Queries.GetAllMaterials;
using RoadRepair.Application.Materials.Queries.GetMaterial;
using RoadRepair.Contracts.Materials.CreateMaterial;
using RoadRepair.Contracts.Materials.GetAllMaterials;
using RoadRepair.Contracts.Materials.GetMaterial;
using RoadRepair.Contracts.Materials.UpdateMaterial;


namespace RoadRepair.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaterialController : ControllerBase
    {
        private IMediator _mediator;
        public MaterialController(IMediator mediatr)
        {
            _mediator = mediatr;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMaterials()
        {
            var query = new GetAllMaterialsQuery();

            var getMaterialsResult = await _mediator.Send(query);

            return getMaterialsResult.MatchFirst(
                material => Ok(new List<MaterialInfo>(material.Select(x => new MaterialInfo(x.Id, x.Name, x.TypeOfMeasureId, x.TypeOfMeasure.Name)).ToList())),
                error => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "There are no Materials"));
        }

        [HttpGet("{materialId:long}")]
        public async Task<IActionResult> GetMaterial(long materialId)
        {
            var query = new GetMaterialQuery(materialId);

            var getMaterialResult = await _mediator.Send(query);

            return getMaterialResult.MatchFirst(
                material => Ok(new GetMaterialResponse(material.Id, material.Name, material.TypeOfMeasureId)),
                error => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "There is no Material with such MaterialId"));
        }

        [HttpPost]
        public async Task<IActionResult> CreateMaterial([FromBody] CreateMaterialRequest request)
        {
            var command = new CreateMaterialCommand(request.Name, request.TypeOfMeasureId, request.OrganizationId);

            var createMaterialResult = await _mediator.Send(command);

            return createMaterialResult.MatchFirst(
                material => Ok(new CreateMaterialResponse(material.Id)),
                error => Problem(error.Description));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMaterial(long id, [FromBody] UpdateMaterialRequest request)
        {
            var command = new UpdateMaterialCommand(id, request.Name, request.TypeOfMeasureId, request.OrganizationId);

            var result = await _mediator.Send(command);

            return result.MatchFirst(
                _ => Ok(null),
                error => Problem(error.Description));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaterial(long id)
        {
            var command = new DeleteMaterialCommand(id);

            var result = await _mediator.Send(command);

            return result.MatchFirst(
                _ => Ok(null),
                error => Problem(error.Description));
        }
    }
}
