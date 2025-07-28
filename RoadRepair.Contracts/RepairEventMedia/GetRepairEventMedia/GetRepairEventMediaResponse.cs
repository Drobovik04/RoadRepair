using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.RepairEventMedia.GetRepairEventMedia
{
    public record GetRepairEventMediaResponse(long Id, long RepairEventId, string FilePath, DateTime CreatedAt, string? Description);
}
