using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Contractors.Commands.UpdateContractor
{
    public record UpdateContractorCommand(long ContractorId, string Name, string Address, string? Email, string? ContactPhone, int UNP) : IRequest<ErrorOr<bool>>;
}
