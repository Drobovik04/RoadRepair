using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using RoadRepair.API.Controllers;
using RoadRepair.Application.Interfaces;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Application.Users.Commands.AssignIdentityAndUser;
using RoadRepair.Application.Users.Commands.CreateUser;
using RoadRepair.Domain.Entities;
using RoadRepair.Infrastructure.Identity;
using RoadRepair.Tests.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Tests.Auth
{
    [TestFixture]
    public class AuthControllerTests : TestDbContextFixture
    {
        [Test]
        public void RegisterNewUser()
        {
            using var context = CreateContext();
            var authServiceMock = new Mock<IAuthService>();
            var mediatorMock = new Mock<IMediator>();
            var userManagerMock = new Mock<UserManager<AppUser>>();
            var roleManagerMock = new Mock<RoleManager<IdentityRole<long>>>();

            var controller = new AuthController(authServiceMock.Object, mediatorMock.Object, userManagerMock.Object, roleManagerMock.Object);

            authServiceMock
                .Setup(x => x.RegisterAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((true, It.IsAny<long>()));

            mediatorMock
                .Setup(x => x.Send(It.IsAny<CreateUserCommand>(), default))
                .ReturnsAsync(new RoadRepair.Domain.Entities.User { Id = 1 });

            mediatorMock
                .Setup(x => x.Send(It.IsAny<AssignIdentityAndUserCommand>(), default))
                .ReturnsAsync(true);

            var result = controller.Register(new Contracts.Auth.RegisterUser.RegisterUserRequest("Ivanov", "Ivan", null, "ivanov", "ivan@mail.com", "Qwe123!", 1));

            Assert.That(result.Result.GetType() == typeof(OkObjectResult));
        }
        [Test]
        public void CreateUserAndIdentityUserAndAssignThem()
        {
            using var context = CreateContext();
        }
    }
}
