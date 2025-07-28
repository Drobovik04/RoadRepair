using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.RepairEventMedia.UpdateRepairEventMedia
{
    public record UpdateRepairEventMediaRequest(long RepairEventId, IFormFile? File, string? Description);
}
