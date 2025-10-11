using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.Reports.GenerateMaterialReportByWorkArea
{
    public record GenerateMaterialReportByWorkAreaRequest(long WorkAreaId, DateOnly? StartDate, DateOnly? EndDate);
}
