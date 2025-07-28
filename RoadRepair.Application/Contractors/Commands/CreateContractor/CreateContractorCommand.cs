using ErrorOr;
using MediatR;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Contractors.Commands.CreateContractor
{
    public record CreateContractorCommand(string Name, string Address, string? Email, string? ContactPhone, int UNP) : IRequest<ErrorOr<Contractor>>;
}
