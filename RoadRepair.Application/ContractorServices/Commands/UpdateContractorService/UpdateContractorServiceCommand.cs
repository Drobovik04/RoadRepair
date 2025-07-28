using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.ContractorServices.Commands.UpdateContractorService
{
    public record UpdateContractorServiceCommand(long ContractorServiceId, long TypeOfServiceId, long ContractorId, decimal Price, string? Description, DateOnly DateOfService, long WorkAreaId) : IRequest<ErrorOr<bool>>;
}
