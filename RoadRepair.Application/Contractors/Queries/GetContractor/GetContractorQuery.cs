using ErrorOr;
using MediatR;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Contractors.Queries.GetContractor
{
    public record GetContractorQuery(long ContractorId) : IRequest<ErrorOr<Contractor>>;
}
