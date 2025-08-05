using ErrorOr;
using MediatR;
using RoadRepair.Domain.Entities;

namespace RoadRepair.Application.Users.Commands.UpdateUser
{
    public record UpdateUserCommand(long Id, string UserName, string? PhoneNumber, string LastName, string? MiddleName, string FirstName, string? Email, long RoleId, long? OrganizationId) : IRequest<ErrorOr<User>>;
}