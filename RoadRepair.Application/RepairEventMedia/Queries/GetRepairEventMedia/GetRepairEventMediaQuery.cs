using ErrorOr;
using MediatR;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.RepairEventMedia.Queries.GetRepairEventMedia
{
    public record GetRepairEventMediaQuery(long RepairEventMediaId) : IRequest<ErrorOr<Domain.Entities.RepairEventMedia>>;
}
