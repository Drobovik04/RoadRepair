using ErrorOr;
using MediatR;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.RepairEvents.Queries.GetRepairEvent
{
    public record GetRepairEventQuery(long RepairEventId) : IRequest<ErrorOr<RepairEvent>>;
}
