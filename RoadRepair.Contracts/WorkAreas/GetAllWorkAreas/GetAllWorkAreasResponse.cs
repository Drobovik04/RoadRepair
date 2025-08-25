using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.WorkAreas.GetAllWorkAreas
{
    public record WorkAreaInfo(long Id, string Name, string? Description, DateOnly? CreatedAt, DateOnly? UpdatedAt, long? ResponsibleId, string? LastName, string? FirstName, string? MiddleName);
    public record GetAllWorkAreasResponse(List<WorkAreaInfo> Values);
}
