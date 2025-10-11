using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.Reports.GenerateAllInOneByWorkArea
{
    public record GenerateAllInOneByWorkAreaRequest(long WorkAreaId, DateOnly? StartDate, DateOnly? EndDate);
}
