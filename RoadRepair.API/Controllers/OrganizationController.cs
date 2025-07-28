using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoadRepair.Application.Organizations.Commands.CreateOrganization;
using RoadRepair.Application.Organizations.Commands.DeleteOrganization;
using RoadRepair.Application.Organizations.Commands.UpdateOrganization;
using RoadRepair.Application.Organizations.Queries.GetAllOrganizations;
using RoadRepair.Application.Organizations.Queries.GetOrganization;
using RoadRepair.Contracts.Organizations.CreateOrganization;
using RoadRepair.Contracts.Organizations.GetAllOrganizations;
using RoadRepair.Contracts.Organizations.GetOrganization;
using RoadRepair.Contracts.Organizations.UpdateOrganization;

namespace RoadRepair.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class OrganizationController : ControllerBase
    {
        private IMediator _mediator;
        public OrganizationController(IMediator mediatr)
        {
            _mediator = mediatr;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrganizations()
        {
            var query = new GetAllOrganizationsQuery();

            var getAllOrganizationsResult = await _mediator.Send(query);

            return getAllOrganizationsResult.MatchFirst(
                organizations => Ok(new List<OrganizationInfo>(organizations.Select(x => new OrganizationInfo(x.Id, x.Name)).ToList())), // пока возрат такой, a little bit weird
                error => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "There is no Organization with such OrganizationId"));
        }

        [HttpGet("{organizationId:long}")]
        public async Task<IActionResult> GetOrganization(long organizationId)
        {
            var query = new GetOrganizationQuery(organizationId);

            var getOrganizationResult = await _mediator.Send(query);

            return getOrganizationResult.MatchFirst(
                organization => Ok(new GetOrganizationResponse(organization.Id, organization.Name, organization.Address, organization.ContactPhone, organization.Email, organization.UNP, organization.ParentId)), // пока возрат такой, a little bit weird
                error => Problem(
                    statusCode: StatusCodes.Status404NotFound, 
                    detail: "There is no Organization with such OrganizationId"));
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrganization([FromBody] CreateOrganizationRequest request)
        {
            var command = new CreateOrganizationCommand(request.Name, request.Address, request.ContactPhone, request.Email, request.UNP, request.ParentId);

            var createOrganizationResult = await _mediator.Send(command);

            return createOrganizationResult.MatchFirst(
                organization => Ok(new CreateOrganizationResponse(organization.Id)),
                error => Problem(error.Description));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrganization(long id, [FromBody] UpdateOrganizationRequest request)
        {
            var command = new UpdateOrganizationCommand(id, request.Name, request.Address, request.ContactPhone, request.Email, request.UNP, request.ParentId);

            var result = await _mediator.Send(command);

            return result.MatchFirst(
                _ => Ok(null),
                error => Problem(error.Description));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrganization(long id)
        {
            var command = new DeleteOrganizationCommand(id);

            var result = await _mediator.Send(command);

            return result.MatchFirst(
                _ => Ok(null),
                error => Problem(error.Description));
        }
    }
}
