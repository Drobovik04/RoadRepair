using ErrorOr;
using MediatR;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.RepairZones.Queries.GetRepairZone
{
    public record GetRepairZoneQuery(long RepairZoneId) : IRequest<ErrorOr<RepairZone>>;
}
