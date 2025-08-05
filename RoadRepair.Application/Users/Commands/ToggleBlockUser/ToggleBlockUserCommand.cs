using ErrorOr;
using MediatR;
using RoadRepair.Domain.Entities;

namespace RoadRepair.Application.Users.Commands.ToggleBlockUser
{
    public record ToggleBlockUserCommand(long Id, bool Status) : IRequest<ErrorOr<bool>>;
}