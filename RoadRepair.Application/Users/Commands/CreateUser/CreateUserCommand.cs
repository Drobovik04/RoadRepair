using ErrorOr;
using MediatR;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Users.Commands.CreateUser
{
    public record CreateUserCommand(string LastName, string? MiddleName, string FirstName, long OrganizationId, long IdentityId) : IRequest<ErrorOr<User>>;
}
