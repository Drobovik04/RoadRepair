using ErrorOr;
using MediatR;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.ContractorServices.Queries.GetAllContractorServices
{
    public record GetAllContractorServicesQuery(long workAreaId) : IRequest<ErrorOr<List<ContractorService>>>;
}
