using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Contractors.Commands.DeleteContractor
{
    public record DeleteContractorCommand(long Id) : IRequest<ErrorOr<bool>>;
}
