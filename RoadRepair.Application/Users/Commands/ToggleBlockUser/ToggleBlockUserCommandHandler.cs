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

namespace RoadRepair.Application.Users.Commands.ToggleBlockUser
{
    public class ToggleBlockUserCommandHandler : IRequestHandler<ToggleBlockUserCommand, ErrorOr<bool>>
    {
        private IUserRepository _userRepository;
        private IAuthService _authService;
        private IUnitOfWork _unitOfWork;
        public ToggleBlockUserCommandHandler(IUserRepository userRepository, IAuthService authService, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _authService = authService;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(ToggleBlockUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindUserByIdAsync(request.Id);

            if (user == null)
            {
                return Error.NotFound(description: "User not found");
            }

            var identityUser = await _authService.GetIdentityUserById(user.IdentityId);

            await _authService.BlockUserById(identityUser.Value.Id, request.Status);

            await _unitOfWork.CommitChangesAsync();

            return true;
        }
    }
}
