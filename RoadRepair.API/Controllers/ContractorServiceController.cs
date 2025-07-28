using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoadRepair.Application.Contractors.Commands.CreateContractor;
using RoadRepair.Application.Contractors.Commands.DeleteContractor;
using RoadRepair.Application.Contractors.Commands.UpdateContractor;
using RoadRepair.Application.Contractors.Queries.GetContractor;
using RoadRepair.Application.ContractorServices.Commands.CreateContractorService;
using RoadRepair.Application.ContractorServices.Commands.DeleteContractorService;
using RoadRepair.Application.ContractorServices.Commands.UpdateContractorService;
using RoadRepair.Application.ContractorServices.Queries.GetAllContractorServices;
using RoadRepair.Application.ContractorServices.Queries.GetContractorService;
using RoadRepair.Application.Workers.Queries.GetAllWorkers;
using RoadRepair.Contracts.Contractors.CreateContractor;
using RoadRepair.Contracts.Contractors.GetContractor;
using RoadRepair.Contracts.Contractors.UpdateContractor;
using RoadRepair.Contracts.ContractorServices.CreateContractorService;
using RoadRepair.Contracts.ContractorServices.GetAllContractorServices;
using RoadRepair.Contracts.ContractorServices.GetContractorService;
using RoadRepair.Contracts.ContractorServices.UpdateContractorService;

namespace RoadRepair.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContractorServiceController : ControllerBase
    {
        private IMediator _mediator;
        public ContractorServiceController(IMediator mediatr)
        {
            _mediator = mediatr;
        }

        [HttpGet("workArea/{workAreaId:long}")]
        public async Task<IActionResult> GetAllWorkers(long workAreaId)
        {
            var query = new GetAllContractorServicesQuery(workAreaId);

            var getContractorServicesResult = await _mediator.Send(query);

            return getContractorServicesResult.MatchFirst(
                contractorServices => Ok(new List<ContractorServiceInfo>(contractorServices.Select(x => new ContractorServiceInfo(x.Id, x.TypeOfServiceId, x.TypeOfService.Name, x.ContractorId, x.Contractor.Name, x.Price, x.Description, x.DateOfService, x.WorkAreaId)))),
                error => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "There are no ContractorServices"));
        }

        [HttpGet("{contractorServiceId:long}")]
        public async Task<IActionResult> GetContractorService(long contractorServiceId)
        {
            var query = new GetContractorServiceQuery(contractorServiceId);

            var getContractorServiceResult = await _mediator.Send(query);

            return getContractorServiceResult.MatchFirst(
                contractorService => Ok(new GetContractorServiceResponse(contractorService.Id, contractorService.TypeOfServiceId, contractorService.ContractorId, contractorService.Price, contractorService.Description, contractorService.DateOfService, contractorService.WorkAreaId)),
                error => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "There is no Service with such ContractorServiceId"));
        }

        [HttpPost]
        public async Task<IActionResult> CreateContractorService([FromBody] CreateContractorServiceRequest request)
        {
            var command = new CreateContractorServiceCommand(request.TypeOfServiceId, request.ContractorId, request.Price, request.Description, request.DateOfService, request.WorkAreaId);

            var createContractorServiceResult = await _mediator.Send(command);

            return createContractorServiceResult.MatchFirst(
                contractorService => Ok(new CreateContractorServiceResponse(contractorService.Id)),
                error => Problem(error.Description));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContractorService(long id, [FromBody] UpdateContractorServiceRequest request)
        {
            var command = new UpdateContractorServiceCommand(id, request.TypeOfServiceId, request.ContractorId, request.Price, request.Description, request.DateOfService, request.WorkAreaId);

            var result = await _mediator.Send(command);

            return result.MatchFirst(
                _ => Ok(null),
                error => Problem(error.Description));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContractorService(long id)
        {
            var command = new DeleteContractorServiceCommand(id);

            var result = await _mediator.Send(command);

            return result.MatchFirst(
                _ => Ok(null),
                error => Problem(error.Description));
        }
    }
}
