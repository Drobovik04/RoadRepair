using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoadRepair.Application.Contractors.Commands.CreateContractor;
using RoadRepair.Application.Contractors.Commands.DeleteContractor;
using RoadRepair.Application.Contractors.Commands.UpdateContractor;
using RoadRepair.Application.Contractors.Queries.GetAllContractors;
using RoadRepair.Application.Contractors.Queries.GetContractor;
using RoadRepair.Contracts.Contractors.CreateContractor;
using RoadRepair.Contracts.Contractors.GetAllContractors;
using RoadRepair.Contracts.Contractors.GetContractor;
using RoadRepair.Contracts.Contractors.UpdateContractor;
using RoadRepair.Contracts.Materials.GetAllMaterials;

namespace RoadRepair.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContractorController : ControllerBase
    {
        private IMediator _mediator;
        public ContractorController(IMediator mediatr)
        {
            _mediator = mediatr;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllContractors()
        {
            var query = new GetAllContractorsQuery();

            var getContractorsResult = await _mediator.Send(query);

            return getContractorsResult.MatchFirst(
                contractors => Ok(new List<ContractorInfo>(contractors.Select(x => new ContractorInfo(x.Id, x.Name, x.Address, x.Email, x.ContactPhone, x.UNP)).ToList())),
                error => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "There are no Contractors"));
        }
        [HttpGet("{contractorId:long}")]
        public async Task<IActionResult> GetContractor(long contractorId)
        {
            var query = new GetContractorQuery(contractorId);

            var getContractorResult = await _mediator.Send(query);

            return getContractorResult.MatchFirst(
                contractor => Ok(new GetContractorResponse(contractor.Id, contractor.Name, contractor.Address, contractor.Email, contractor.ContactPhone, contractor.UNP)),
                error => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "There is no Contractor with such ContractorId"));
        }

        [HttpPost]
        public async Task<IActionResult> CreateContractor([FromBody] CreateContractorRequest request)
        {
            var command = new CreateContractorCommand(request.Name, request.Address, request.Email, request.ContactPhone, request.UNP);

            var createContractorResult = await _mediator.Send(command);

            return createContractorResult.MatchFirst(
                contractor => Ok(new CreateContractorResponse(contractor.Id)),
                error => Problem(error.Description));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContractor(long id, [FromBody] UpdateContractorRequest request)
        {
            var command = new UpdateContractorCommand(id, request.Name, request.Address, request.Email, request.ContactPhone, request.UNP);

            var result = await _mediator.Send(command);

            return result.MatchFirst(
                _ => Ok(null),
                error => Problem(error.Description));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContractor(long id)
        {
            var command = new DeleteContractorCommand(id);

            var result = await _mediator.Send(command);

            return result.MatchFirst(
                _ => Ok(null),
                error => Problem(error.Description));
        }
    }
}
