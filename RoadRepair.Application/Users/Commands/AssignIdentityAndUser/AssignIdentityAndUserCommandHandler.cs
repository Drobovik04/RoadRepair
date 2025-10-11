using ErrorOr;
using MediatR;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Application.Interfaces;
using RoadRepair.Application.Users.Commands.AssignIdentityAndUser;

namespace RoadRepair.Application.Users.Commands.AssignIdentityAndUser
{
    public class AssignIdentityAndUserCommandHandler : IRequestHandler<AssignIdentityAndUserCommand, ErrorOr<bool>>
    {
        private IIdentityRepository _identityRepository;
        private IUserRepository _userRepository;
        private IUnitOfWork _unitOfWork;
        public AssignIdentityAndUserCommandHandler(IIdentityRepository identityRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _identityRepository = identityRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(AssignIdentityAndUserCommand request, CancellationToken cancellationToken)
        {
            if (!_identityRepository.IsExists(request.IdentityId))
            {
                return Error.Failure(description: "Нет такого Identity");
            }


            var orgUser = await _userRepository.FindUserByIdAsync(request.UserId);

            if (orgUser == null)
            {
                return Error.Failure(description: "Нет такого пользователя");
            }

            orgUser.IdentityId = request.IdentityId;

            _userRepository.UpdateUser(orgUser);

            await _unitOfWork.CommitChangesAsync();

            return true;
        }
    }
}
