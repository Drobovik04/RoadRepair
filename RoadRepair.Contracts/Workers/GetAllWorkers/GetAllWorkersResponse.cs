using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.Workers.GetAllWorkers
{
    public record WorkerInfo(long Id, long OrganizationId, string LastName, string? MiddleName, string FirstName, DateOnly HiredAt, DateOnly? FiredAt, long? PositionId, string? PositionName);
    public record GetAllWorkersResponse(List<WorkerInfo> Values);
}
