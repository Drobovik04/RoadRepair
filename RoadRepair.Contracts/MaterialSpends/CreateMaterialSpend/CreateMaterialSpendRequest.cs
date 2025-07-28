using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.MaterialSpends.CreateMaterialSpend
{
    public record CreateMaterialSpendRequest(long MaterialId, decimal Price, double Volume, long RepairEventId, long ContractorId);
}
