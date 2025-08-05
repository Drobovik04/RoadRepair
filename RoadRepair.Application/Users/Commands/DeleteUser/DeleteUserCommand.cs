using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Users.Commands.DeleteUser
{
    public record DeleteUserCommand(long Id) : IRequest<ErrorOr<bool>>;
}
