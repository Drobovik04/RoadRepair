using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.MaterialSpends.Commands.UpdateMaterialSpend
{
    public record UpdateMaterialSpendCommand(long MaterialSpendId, long MaterialId, decimal Price, double Volume, long RepairEventId, long ContractorId) : IRequest<ErrorOr<bool>>;
}
