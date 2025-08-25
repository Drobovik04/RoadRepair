using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Contracts.Workers.GetWorker
{
    public record GetWorkerResponse(long Id, string LastName, string? MiddleName, string FirstName, DateOnly HiredAt, DateOnly? FiredAt, long? PositionId);
}
