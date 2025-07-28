using Microsoft.AspNetCore.Http;

namespace RoadRepair.Contracts.RepairEventMedia.CreateRepairEventMedia
{
    public record CreateRepairEventMediaRequest(long RepairEventId, IFormFile File, string? Description);
}
