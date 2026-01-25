using OfficeOpenXml;
using OfficeOpenXml.Style;
using RoadRepair.Application.Interfaces;
using RoadRepair.Application.Interfaces.Reports;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Infrastructure.Reports
{
    public class MaterialReportBuilder : IReportBuilder
    {
        private readonly IExcelReportService _excelService;

        public MaterialReportBuilder(IExcelReportService excelService)
        {
            _excelService = excelService;
        }

        public Stream BuildMaterialReport(WorkArea workArea, User userInfo, DateOnly? startDate, DateOnly? endDate)
        {
            using var excelBlank = _excelService.LoadTemplate("RoadRepair.Infrastructure.Reports.Blanks.MaterialsReport.xlsx");

            ExcelWorksheet sheet = excelBlank.Workbook.Worksheets["List1"];

            int row = 15, col = 2;
            decimal sumRepairEvents = 0;

            sheet.Cells[10, 1].Value = startDate == null && endDate == null ? "за весь период": $"с {startDate.ToString()} по {endDate.ToString()}";
            sheet.Cells[11, 1].Value += workArea.Name;
            sheet.Cells[12, 1].Value += $"{workArea.Responsible.LastName} {workArea.Responsible.FirstName} {userInfo.MiddleName}";
            sheet.Cells[20, 2].Value += $"{userInfo.LastName} {userInfo.FirstName[0]}.{userInfo.MiddleName?[0]}";

            var allRows = workArea.RepairZones.Sum(x => 1 + x.RepairEvents.Where(re => re.StartedAt >= startDate && re.EndedAt <= endDate).Sum(re => 1 + re.MaterialSpends.Count + 1) + 1);

            if(allRows != 0)
            {
                sheet.InsertRow(row + 1, allRows, row);
            }


            for (int i = 0; i < workArea.RepairZones.Count; i++)
            {
                var repairZone = workArea.RepairZones[i];

                sheet.Cells[row, col - 1].Value = i + 1;
                sheet.Cells[row, col].Value = repairZone.Name;

                row++;

                decimal sumAllMaterialsByZone = 0;
                for (int j = 0; j < repairZone.RepairEvents.Count; j++)
                {
                    var repairEvent = repairZone.RepairEvents[j];

                    if (startDate != null && endDate != null && !(repairEvent.StartedAt >= startDate && repairEvent.EndedAt <= endDate))
                    {
                        continue;
                    }

                    sheet.Cells[row, col + 1].Value = repairEvent.TypeOfRepair.Name;
                    sheet.Cells[row, col + 2].Value = repairEvent.StartedAt.ToString();
                    sheet.Cells[row, col + 3].Value = repairEvent.EndedAt.ToString();
                    row++;

                    decimal sumAllMaterials = 0;
                    for (int k = 0; k < repairEvent.MaterialSpends.Count; k++)
                    {
                        var materialSpend = repairEvent.MaterialSpends[k];

                        sheet.Cells[row, col + 4].Value = materialSpend.Contractor.Name;
                        sheet.Cells[row, col + 5].Value = materialSpend.Material.Name;
                        sheet.Cells[row, col + 6].Value = materialSpend.Material.TypeOfMeasure.ShortName;
                        sheet.Cells[row, col + 7].Value = materialSpend.Price;
                        sheet.Cells[row, col + 8].Value = materialSpend.Volume;
                        sheet.Cells[row, col + 9].Value = materialSpend.Price * Convert.ToDecimal(materialSpend.Volume);

                        sumAllMaterials += materialSpend.Price * Convert.ToDecimal(materialSpend.Volume);
                        row++;
                    }
                    sheet.Cells[row, col].Value = "Итого по ремонту";
                    sheet.Cells[row, col].Style.Font.Bold = true;

                    sheet.Cells[row, col + 9].Value = sumAllMaterials;
                    sheet.Cells[row, col + 9].Style.Font.Bold = true;
                    sumAllMaterialsByZone += sumAllMaterials;
                    row++;
                }

                sheet.Cells[row, col].Value = "Итого по зоне";
                sheet.Cells[row, col].Style.Font.Bold = true;

                sheet.Cells[row, col + 9].Value = sumAllMaterialsByZone;
                sheet.Cells[row, col + 9].Style.Font.Bold = true;
                sumRepairEvents += sumAllMaterialsByZone;
                row++;
            }

            sheet.Cells[row, col].Value = "Итого по всем зонам";
            sheet.Cells[row, col].Style.Font.Bold = true;
            sheet.Cells[row, col + 9].Value = sumRepairEvents;
            sheet.Cells[row, col + 9].Style.Font.Bold = true;

            var tableRange = sheet.Cells[14, 1, row, col + 9];

            tableRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            tableRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            tableRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            tableRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            tableRange.AutoFitColumns();

            sheet.InsertRow(row + 1, 1);

            row += 2; // небольшой отступ после "Итого по всем зонам"
            sheet.Cells[row, col].Value = "ИТОГО ПО МАТЕРИАЛАМ";
            sheet.Cells[row, col].Style.Font.Bold = true;
            row++;

            // Собираем все материалы
            var allMaterialSpends = workArea.RepairZones
                .SelectMany(z => z.RepairEvents)
                .Where(re => !(startDate != null && endDate != null && !(re.StartedAt >= startDate && re.EndedAt <= endDate)))
                .SelectMany(re => re.MaterialSpends)
                .ToList();

            // Группируем по материалу
            var groupedMaterials = allMaterialSpends
                .GroupBy(ms => ms.Material)
                .Select(g => new
                {
                    Material = g.Key,
                    TotalVolume = g.Sum(x => x.Volume),
                    TotalCost = g.Sum(x => x.Price * Convert.ToDecimal(x.Volume))
                })
                .OrderBy(x => x.Material.Name) // для красоты
                .ToList();

            // Заголовки
            row++;

            sheet.Cells[row, col - 1].Value = "№";
            sheet.Cells[row, col].Value = "Материал";
            sheet.Cells[row, col + 1].Value = "Ед. изм.";
            sheet.Cells[row, col + 2].Value = "Объем";
            sheet.Cells[row, col + 3].Value = "Стоимость";
            sheet.Cells[row, col - 1, row, col + 3].Style.Font.Bold = true;
            sheet.Cells[row, col - 1, row, col + 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            row++;

            sheet.Cells[row, col - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells[row, col - 1].Style.Font.Bold = true;
            sheet.Cells[row, col + 1, row, col + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            if (groupedMaterials.Count != 0)
            {
                sheet.InsertRow(row + 1, groupedMaterials.Count + 4, row);
            }

            // Данные
            for(int i = 0; i < groupedMaterials.Count; i++)
            {
                var item = groupedMaterials[i];

                sheet.Cells[row, col - 1].Value = i + 1;
                sheet.Cells[row, col].Value = item.Material.Name;
                sheet.Cells[row, col + 1].Value = item.Material.TypeOfMeasure.ShortName;
                sheet.Cells[row, col + 2].Value = item.TotalVolume;
                sheet.Cells[row, col + 3].Value = item.TotalCost;
                row++;
            }

            // Общая сумма по всем материалам
            sheet.Cells[row, col].Value = "Всего по материалам";
            sheet.Cells[row, col].Style.Font.Bold = true;
            sheet.Cells[row, col + 3].Value = groupedMaterials.Sum(x => x.TotalCost);
            sheet.Cells[row, col + 3].Style.Font.Bold = true;

            tableRange = sheet.Cells[row - groupedMaterials.Count - 1, 1, row, col + 3];

            tableRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            tableRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            tableRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            tableRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            return _excelService.SaveToStream(excelBlank);
        }

        public Stream BuildThirdPartyServicesReport(WorkArea workArea, User userInfo, DateOnly? startDate, DateOnly? endDate)
        {
            using var excelBlank = _excelService.LoadTemplate("RoadRepair.Infrastructure.Reports.Blanks.ThirdPartyServicesReport.xlsx");

            ExcelWorksheet sheet = excelBlank.Workbook.Worksheets["List1"];

            int row = 15, col = 2;
            decimal sumServicePrices = 0;

            sheet.Cells[10, 1].Value = startDate == null && endDate == null ? "за весь период" : $"с {startDate.ToString()} по {endDate.ToString()}";
            sheet.Cells[11, 1].Value += workArea.Name;
            sheet.Cells[12, 1].Value += $"{workArea.Responsible.LastName} {workArea.Responsible.FirstName} {userInfo.MiddleName}";
            sheet.Cells[20, 2].Value += $"{userInfo.LastName} {userInfo.FirstName[0]}.{userInfo.MiddleName?[0]}";

            var allRows = workArea.ContractorServices.Where(x => x.DateOfService >= startDate && x.DateOfService <= endDate).Count();

            if(allRows != 0)
            {
                sheet.InsertRow(row + 1, allRows, row);
            }

            long counter = 1;

            for (int i = 0; i < workArea.ContractorServices.Count; i++)
            {
                var contractorService = workArea.ContractorServices[i];

                if (startDate != null && endDate != null && !(contractorService.DateOfService >= startDate && contractorService.DateOfService <= endDate))
                {
                    continue;
                }

                sheet.Cells[row, col - 1].Value = counter++;
                sheet.Cells[row, col].Value = contractorService.TypeOfService.Name;
                sheet.Cells[row, col + 1].Value = contractorService.Description;
                sheet.Cells[row, col + 2].Value = contractorService.Contractor.UNP;
                sheet.Cells[row, col + 3].Value = contractorService.Contractor.Name;
                sheet.Cells[row, col + 4].Value = contractorService.Price;

                sumServicePrices += contractorService.Price;

                row++;
            }

            sheet.Cells[row, col].Value = "Итого";
            sheet.Cells[row, col].Style.Font.Bold = true;
            sheet.Cells[row, col + 4].Value = sumServicePrices;
            sheet.Cells[row, col + 4].Style.Font.Bold = true;

            var tableRange = sheet.Cells[14, 1, row, col + 4];

            tableRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            tableRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            tableRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            tableRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            tableRange.AutoFitColumns();

            return _excelService.SaveToStream(excelBlank);
        }

        public Stream BuildWorkTimeReport(WorkArea workArea, User userInfo, DateOnly? startDate, DateOnly? endDate)
        {
            using var excelBlank = _excelService.LoadTemplate("RoadRepair.Infrastructure.Reports.Blanks.WorkTimeReport.xlsx");

            ExcelWorksheet sheet = excelBlank.Workbook.Worksheets["List1"];

            int row = 15, col = 2;

            sheet.Cells[10, 1].Value = startDate == null && endDate == null ? "за весь период" : $"с {startDate.ToString()} по {endDate.ToString()}";
            sheet.Cells[11, 1].Value += workArea.Name;
            sheet.Cells[12, 1].Value += $"{workArea.Responsible.LastName} {workArea.Responsible.FirstName} {userInfo.MiddleName}";
            sheet.Cells[20, 2].Value += $"{userInfo.LastName} {userInfo.FirstName[0]}.{userInfo.MiddleName?[0]}";

            var allRows = workArea.WorkAreaWorkers.Count;

            var allDates = workArea.WorkAreaWorkers
                .SelectMany(waw => waw.WorkTimes)
                .Where(wt => (startDate == null || wt.DayOfWork >= startDate) &&
                             (endDate == null || wt.DayOfWork <= endDate))
                .Select(wt => wt.DayOfWork)
                .Distinct()
                .OrderBy(d => d)
                .ToList();

            for (int i = 0; i < allDates.Count; i++)
            {
                sheet.Cells[row - 1, col + 1 + i].Value = allDates[i].ToString("dd.MM");
            }

            sheet.Cells[row - 1, col + 1 + allDates.Count].Value = "Сумма";

            if (allRows != 0)
            {
                sheet.InsertRow(row + 1, allRows, row);
            }

            double allHours = 0;

            for (int i = 0; i < workArea.WorkAreaWorkers.Count; i++)
            {
                var workAreaWorker = workArea.WorkAreaWorkers[i];
                var worker = workAreaWorker.Worker;
                double hoursForWorker = 0;

                var workTimesDict = workAreaWorker.WorkTimes
                    .Where(wt => allDates.Contains(wt.DayOfWork))
                    .GroupBy(wt => wt.DayOfWork)
                    .ToDictionary(g => g.Key, g => g.Sum(x => x.Hours));

                for (int j = 0; j < allDates.Count; j++)
                {
                    if (workTimesDict.TryGetValue(allDates[j], out var hours))
                    {
                        sheet.Cells[row, col + 1 + j].Value = hours;
                        hoursForWorker += hours;
                    }
                }


                sheet.Cells[row, col - 1].Value = i + 1;
                sheet.Cells[row, col].Value = $"{worker.LastName} {worker.FirstName} {worker.MiddleName}";

                sheet.Cells[row, col + 1 + allDates.Count].Value = hoursForWorker;
                sheet.Cells[row, col + 1 + allDates.Count].Style.Font.Bold = true;

                allHours += hoursForWorker;

                row++;
            }

            for (int j = 0; j < allDates.Count; j++)
            {
                double sumByDay = workArea.WorkAreaWorkers
                    .SelectMany(waw => waw.WorkTimes)
                    .Where(wt => wt.DayOfWork == allDates[j]
                              && (startDate == null || wt.DayOfWork >= startDate)
                              && (endDate == null || wt.DayOfWork <= endDate))
                    .Sum(wt => wt.Hours);

                sheet.Cells[row, col + 1 + j].Value = sumByDay;
                sheet.Cells[row, col + 1 + j].Style.Font.Bold = true;
            }

            sheet.Cells[row, col].Value = "Итого";
            sheet.Cells[row, col].Style.Font.Bold = true;
            sheet.Cells[row, col + 1 + allDates.Count].Value = allHours;
            sheet.Cells[row, col + 1 + allDates.Count].Style.Font.Bold = true;

            var tableRange = sheet.Cells[14, 1, row, col + 1 + allDates.Count];

            tableRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            tableRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            tableRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            tableRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            tableRange.AutoFitColumns();

            return _excelService.SaveToStream(excelBlank);
        }

        public Stream BuildAllInOneReport(WorkArea workArea, User userInfo, DateOnly? startDate, DateOnly? endDate)
        {
            ExcelPackage excelBlank = new ExcelPackage();

            ExcelPackage report1, report2, report3;

            report1 = new ExcelPackage(BuildMaterialReport(workArea, userInfo, startDate, endDate));
            report2 = new ExcelPackage(BuildThirdPartyServicesReport(workArea, userInfo, startDate, endDate));
            report3 = new ExcelPackage(BuildWorkTimeReport(workArea, userInfo, startDate, endDate));

            ExcelWorksheet sh1, sh2, sh3;

            sh1 = report1.Workbook.Worksheets.First();
            sh2 = report2.Workbook.Worksheets.First();
            sh3 = report3.Workbook.Worksheets.First();

            NormalizeWorksheetStyle(sh1);
            NormalizeWorksheetStyle(sh2);
            NormalizeWorksheetStyle(sh3);

            excelBlank.Workbook.Worksheets.Add("Материалы", sh1);
            excelBlank.Workbook.Worksheets.Add("Услуги", sh2);
            excelBlank.Workbook.Worksheets.Add("Сотрудники", sh3);

            return _excelService.SaveToStream(excelBlank);
        }

        private void NormalizeWorksheetStyle(ExcelWorksheet sheet, string fontName = "Calibri", int fontSize = 11)
        {
            if (sheet.Dimension != null)
            {
                var cells = sheet.Cells[sheet.Dimension.Address];
                cells.Style.Font.Name = fontName;
                cells.Style.Font.Size = fontSize;
            }
        }
    }

}
