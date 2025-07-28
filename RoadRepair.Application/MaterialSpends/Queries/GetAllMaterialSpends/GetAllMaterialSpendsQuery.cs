using ErrorOr;
using MediatR;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.MaterialSpends.Queries.GetAllMaterialSpends
{
    public record GetAllMaterialSpendsQuery(long RepairEventId) : IRequest<ErrorOr<List<MaterialSpend>>>;
}
