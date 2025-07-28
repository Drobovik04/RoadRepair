using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.Workers.UpdateWorker
{
    public record UpdateWorkerRequest(long OrganizationId, string LastName, string? MiddleName, string FirstName, DateOnly HiredAt, DateOnly? FiredAt, long PositionId);
}
