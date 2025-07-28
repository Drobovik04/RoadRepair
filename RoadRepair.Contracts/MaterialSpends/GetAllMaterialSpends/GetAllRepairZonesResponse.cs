using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.MaterialSpends.GetAllMaterialSpends
{
    public record MaterialSpendInfo(long Id, long MaterialId, string MaterialName, long TypeOfMeasureId, string TypeOfMeasureName, decimal Price, double Volume, long RepairEventId, long ContractorId, string ContractorName);
    public record GetAllMaterialSpendsResponse(List<MaterialSpendInfo> Values);
}
