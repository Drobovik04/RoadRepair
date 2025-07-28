using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.ContractorServices.Commands.DeleteContractorService
{
    public record DeleteContractorServiceCommand(long Id) : IRequest<ErrorOr<bool>>;
}
