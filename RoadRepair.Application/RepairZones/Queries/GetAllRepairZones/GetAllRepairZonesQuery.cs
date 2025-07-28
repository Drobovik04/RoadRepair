using ErrorOr;
using MediatR;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.RepairZones.Queries.GetAllRepairZones
{
    public record GetAllRepairZonesQuery(long WorkAreaId) : IRequest<ErrorOr<List<RepairZone>>>;
}
