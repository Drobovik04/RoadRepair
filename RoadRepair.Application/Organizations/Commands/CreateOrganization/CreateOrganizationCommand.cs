using ErrorOr;
using MediatR;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Organizations.Commands.CreateOrganization
{
    public record CreateOrganizationCommand(string Name, string? Address, string? ContactPhone, string? Email, int UNP, long? ParentId) : IRequest<ErrorOr<Organization>>;
}
