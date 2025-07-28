using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.RepairEventMedia.GetAllRepairEventMedia
{
    public record RepairEventMediaInfo(long Id, long RepairEventId, string FilePath, DateTime CreatedAt, string? Description);
    public record GetAllRepairZonesResponse(List<RepairEventMediaInfo> Values);
}
