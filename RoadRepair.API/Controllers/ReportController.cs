using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoadRepair.Application.Reports.Queries.GenerateAllInOneReportByWorkArea;
using RoadRepair.Application.Reports.Queries.GenerateMaterialReportByWorkArea;
using RoadRepair.Application.Reports.Queries.GenerateThirdPartServicesReportByWorkArea;
using RoadRepair.Application.Reports.Queries.GenerateWorkTimeReportByWorkArea;
using RoadRepair.Contracts.Reports.GenerateAllInOneByWorkArea;
using RoadRepair.Contracts.Reports.GenerateMaterialReportByWorkArea;
using RoadRepair.Contracts.Reports.GenerateThirdPartyServicesByWorkArea;
using RoadRepair.Contracts.Reports.GenerateWorkTimeByWorkArea;
using System.IO;
using System.Security.Claims;

namespace RoadRepair.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin, Manager")]
    public class ReportController : ControllerBase
    {
        private IMediator _mediator;
        public ReportController(IMediator mediatr)
        {
            _mediator = mediatr;
        }
        [HttpGet("materialReport")]
        public async Task<IActionResult> GenerateMaterialReportByWorkArea([FromQuery] GenerateMaterialReportByWorkAreaRequest request)
        {
            var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var query = new GenerateMaterialReportByWorkAreaQuery(request.WorkAreaId, request.StartDate, request.EndDate, userId);

            var reportResult = await _mediator.Send(query);


            return reportResult.MatchFirst<IActionResult>(
                materialSpend => File(materialSpend, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "MaterialsReport.xlsx"),
                error => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "Cannot generate report for some reasons"));
        }

        [HttpGet("thirdPartyServicesReport")]
        public async Task<IActionResult> GenerateThirdPartyServicesByWorkArea([FromQuery] GenerateThirdPartyServicesByWorkAreaRequest request)
        {
            var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var query = new GenerateThirdPartServicesReportByWorkAreaQuery(request.WorkAreaId, request.StartDate, request.EndDate, userId);

            var reportResult = await _mediator.Send(query);


            return reportResult.MatchFirst<IActionResult>(
                services => File(services, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ServicesReport.xlsx"),
                error => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "Cannot generate report for some reasons"));
        }

        [HttpGet("workTimeReport")]
        public async Task<IActionResult> GenerateWorkTimeWorkArea([FromQuery] GenerateWorkTimeByWorkAreaRequest request)
        {
            var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var query = new GenerateWorkTimeReportByWorkAreaQuery(request.WorkAreaId, request.StartDate, request.EndDate, userId);

            var reportResult = await _mediator.Send(query);


            return reportResult.MatchFirst<IActionResult>(
                services => File(services, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ServicesReport.xlsx"),
                error => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "Cannot generate report for some reasons"));
        }

        [HttpGet("allInOneReport")]
        public async Task<IActionResult> GenerateAllInOneWorkArea([FromQuery] GenerateAllInOneByWorkAreaRequest request)
        {
            var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var query = new GenerateAllInOneReportByWorkAreaQuery(request.WorkAreaId, request.StartDate, request.EndDate, userId);

            var reportResult = await _mediator.Send(query);


            return reportResult.MatchFirst<IActionResult>(
                services => File(services, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ServicesReport.xlsx"),
                error => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "Cannot generate report for some reasons"));
        }
    }
}
