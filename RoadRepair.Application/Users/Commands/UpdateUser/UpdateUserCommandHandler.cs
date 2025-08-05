using ErrorOr;
using MediatR;
using RoadRepair.Application.Interfaces;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Users.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, ErrorOr<User>>
    {
        private IUserRepository _userRepository;
        private IAuthService _authService;
        private IUnitOfWork _unitOfWork;
        public UpdateUserCommandHandler(IUserRepository userRepository, IAuthService authService, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _authService = authService;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<User>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindUserByIdAsync(request.Id);

            if (user == null)
            {
                return Error.NotFound(description: "User not found");
            }

            var identityUser = await _authService.GetIdentityUserById(user.IdentityId);

            user.LastName = request.LastName;
            user.FirstName = request.FirstName;
            user.MiddleName = request.MiddleName;

            identityUser.Value.UserName = request.UserName;
            identityUser.Value.PhoneNumber = request.PhoneNumber;
            identityUser.Value.Email = request.Email;

            _userRepository.UpdateUser(user);
            await _authService.UpdateRoleForUser(identityUser.Value.Id, request.RoleId);

            await _authService.UpdateIdentityUser(identityUser.Value);

            await _unitOfWork.CommitChangesAsync();

            return user;
        }
    }
}
