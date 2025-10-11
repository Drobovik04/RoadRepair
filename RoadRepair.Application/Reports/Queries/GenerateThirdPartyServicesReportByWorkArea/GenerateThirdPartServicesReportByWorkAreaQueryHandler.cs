using ErrorOr;
using MediatR;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using RoadRepair.Application.Interfaces;
using RoadRepair.Application.Interfaces.Reports;
using RoadRepair.Application.Interfaces.Repositories;
using System.Reflection;

namespace RoadRepair.Application.Reports.Queries.GenerateThirdPartServicesReportByWorkArea
{
    public class GenerateThirdPartServicesReportByWorkAreaQueryHandler : IRequestHandler<GenerateThirdPartServicesReportByWorkAreaQuery, ErrorOr<Stream>>
    {
        private IWorkAreaRepository _workAreaRepository;
        private IUserRepository _userRepository;
        private IReportBuilder _materialReportBuilder;
        private IFileService _fileService;
        public GenerateThirdPartServicesReportByWorkAreaQueryHandler(IWorkAreaRepository workAreaRepository, IUserRepository userRepository, IReportBuilder materialReportBuilder, IFileService fileService)
        {
            _workAreaRepository = workAreaRepository;
            _userRepository = userRepository;
            _materialReportBuilder = materialReportBuilder;
            _fileService = fileService;
        }

        public async Task<ErrorOr<Stream>> Handle(GenerateThirdPartServicesReportByWorkAreaQuery query, CancellationToken cancellationToken)
        {
            var workArea = await _workAreaRepository.GetWorkAreaWithAllDependencies(query.workAreaId);

            if (workArea == null)
            {
                return Error.NotFound(description: "WorkArea or info are not found");
            }

            var userInfo = await _userRepository.FindUserByAppUserIdAsync(query.createdByUser);

            var reportStream = _materialReportBuilder.BuildThirdPartyServicesReport(workArea, userInfo, query.startDate, query.endDate);

            return reportStream;
        }
    }
}
