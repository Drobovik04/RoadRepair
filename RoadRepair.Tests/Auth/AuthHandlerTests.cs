using Moq;
using NUnit.Framework;
using RoadRepair.Application.Interfaces;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Application.Users.Commands.CreateUser;
using RoadRepair.Domain.Entities;
using RoadRepair.Tests.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Tests.Auth
{
    [TestFixture]
    public class AuthHandlerTests : TestDbContextFixture
    {
        [Test]
        public async Task CreateUserCommandHandler_CreatesUser_AndCommits()
        {
            // Arrange
            var userRepo = new Mock<IUserRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();

            var handler = new CreateUserCommandHandler(userRepo.Object, unitOfWork.Object);

            var command = new CreateUserCommand("Иванов", null, "Иван", 1, 2);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            userRepo.Verify(x => x.AddNewUserAsync(It.Is<RoadRepair.Domain.Entities.User>(u =>
                u.LastName == "Иванов" &&
                u.FirstName == "Иван" &&
                u.IdentityId == 2
            )), Times.Once);

            unitOfWork.Verify(x => x.CommitChangesAsync(), Times.Once);

            Assert.That(!result.IsError);
            Assert.That("Иван" == result.Value.FirstName);
        }
    }
}
