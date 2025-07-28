using ErrorOr;
using MediatR;
using RoadRepair.Domain.Entities;

namespace RoadRepair.Application.MaterialSpends.Commands.CreateMaterialSpend
{
    public record CreateMaterialSpendCommand(long MaterialId, decimal Price, double Volume, long RepairEventId, long ContractorId) : IRequest<ErrorOr<MaterialSpend>>;
}
