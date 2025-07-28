using ErrorOr;
using MediatR;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Users.Commands.AssignIdentityAndUser
{
    public record AssignIdentityAndUserCommand(long UserId, long IdentityId) : IRequest<ErrorOr<bool>>;
}
