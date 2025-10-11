using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Reports.Queries.GenerateMaterialReportByWorkArea
{
    public record GenerateMaterialReportByWorkAreaQuery(long workAreaId, DateOnly? startDate, DateOnly? endDate, long createdByUser) : IRequest<ErrorOr<Stream>>;
}
