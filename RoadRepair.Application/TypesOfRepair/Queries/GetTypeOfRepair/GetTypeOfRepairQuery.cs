using ErrorOr;
using MediatR;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.TypesOfRepair.Queries.GetTypeOfRepair
{
    public record GetTypeOfRepairQuery(long TypeOfRepairId) : IRequest<ErrorOr<TypeOfRepair>>;
}
