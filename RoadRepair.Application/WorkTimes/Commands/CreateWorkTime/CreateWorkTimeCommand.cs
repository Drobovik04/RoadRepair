using ErrorOr;
using MediatR;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.WorkTimes.Commands.CreateWorkTime
{
    public record CreateWorkTimeCommand(DateOnly DayOfWork, double Hours, long RepairEventWorkerId) : IRequest<ErrorOr<WorkTime>>;
}
