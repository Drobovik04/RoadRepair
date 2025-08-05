using ErrorOr;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Application.Interfaces;
using RoadRepair.Application.Organizations.Commands.DeleteOrganization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace RoadRepair.Application.Users.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, ErrorOr<bool>>
    {
        private IUserRepository _userRepository;
        private IAuthService _authService;
        private IUnitOfWork _unitOfWork;
        public DeleteUserCommandHandler(IUserRepository userRepository, IAuthService authService, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _authService = authService;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindUserByIdAsync(request.Id);

            if (user == null)
            {
                return Error.NotFound(description: "User not found");
            }

            var identityUser = _authService.GetIdentityUserById(user.IdentityId);

            await _authService.DeleteIdentityUser(user.IdentityId);

            _userRepository.DeleteUser(user);

            await _unitOfWork.CommitChangesAsync();

            return true;
        }
    }
}
