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

namespace RoadRepair.Application.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ErrorOr<User>>
    {
        private IUserRepository _userRepository;
        private IUnitOfWork _unitOfWork;
        public CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<User>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                LastName = request.LastName,
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                IdentityId = request.IdentityId,
                CreatedAt = DateTime.UtcNow,
            };

            await _userRepository.AddNewUserAsync(user);

            await _unitOfWork.CommitChangesAsync();

            return user;
        }
    }
}
