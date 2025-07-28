using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.RepairEventMedia.Queries.GetAllRepairMedia
{
    public record GetAllRepairEventMediaQuery(long repairEventId) : IRequest<ErrorOr<List<Domain.Entities.RepairEventMedia>>>;
}
