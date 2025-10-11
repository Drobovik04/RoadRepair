using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Interfaces
{
    public interface IExcelReportService
    {
        ExcelPackage LoadTemplate(string resourceName);
        MemoryStream SaveToStream(ExcelPackage package);
    }
}
