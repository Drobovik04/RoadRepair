using ErrorOr;
using MediatR;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.RepairEvents.Queries.GetAllRepairEventsByRepairZoneId
{
    public record GetAllRepairEventsByRepairZoneIdQuery(long repairZoneId) : IRequest<ErrorOr<List<RepairEvent>>>;
}
