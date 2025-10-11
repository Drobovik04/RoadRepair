using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Interfaces.Reports
{
    public interface IReportBuilder
    {
        Stream BuildMaterialReport(WorkArea workArea, User userInfo, DateOnly? startDate, DateOnly? endDate);
        Stream BuildThirdPartyServicesReport(WorkArea workArea, User userInfo, DateOnly? startDate, DateOnly? endDate);
        Stream BuildWorkTimeReport(WorkArea workArea, User userInfo, DateOnly? startDate, DateOnly? endDate);
        Stream BuildAllInOneReport(WorkArea workArea, User userInfo, DateOnly? startDate, DateOnly? endDate);
    }

}
