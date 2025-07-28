using ErrorOr;
using MediatR;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.ContractorServices.Commands.CreateContractorService
{
    public record CreateContractorServiceCommand(long TypeOfServiceId, long ContractorId, decimal Price, string? Description, DateOnly DateOfService, long WorkAreaId) : IRequest<ErrorOr<ContractorService>>;
}
