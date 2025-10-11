using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.Reports.GenerateThirdPartyServicesByWorkArea
{
    public record GenerateThirdPartyServicesByWorkAreaRequest(long WorkAreaId, DateOnly? StartDate, DateOnly? EndDate);
}
