using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Workers.Commands.DeleteWorker
{
    public record DeleteWorkerCommand(long Id) : IRequest<ErrorOr<bool>>;
}
