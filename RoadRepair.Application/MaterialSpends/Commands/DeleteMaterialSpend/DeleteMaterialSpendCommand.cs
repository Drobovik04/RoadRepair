using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.MaterialSpends.Commands.DeleteMaterialSpend
{
    public record DeleteMaterialSpendCommand(long Id) : IRequest<ErrorOr<bool>>;
}
