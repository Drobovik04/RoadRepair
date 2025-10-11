using OfficeOpenXml;
using RoadRepair.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Infrastructure.Services
{
    public class ExcelReportService : IExcelReportService
    {
        public ExcelPackage LoadTemplate(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream(resourceName);

            var excelBlank = new ExcelPackage(stream);
            return excelBlank;
        }

        public MemoryStream SaveToStream(ExcelPackage package)
        {
            MemoryStream outStream = new MemoryStream();
            package.SaveAs(outStream);

            outStream.Position = 0;

            return outStream;
        }
    }
}
