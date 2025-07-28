using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Organizations.Commands.DeleteOrganization
{
    public record DeleteOrganizationCommand(long Id) : IRequest<ErrorOr<bool>>;
}
