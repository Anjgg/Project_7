using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using P7CreateRestApi.Controllers;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Dto;
using P7CreateRestApi.Services;

namespace P7CreateRestApi_Tests.Controllers
{
    [TestClass]
    public class AuthentificationControllerTests
    {
        [TestMethod]
        public async Task Login_ReturnUnauthorized_WhenEmailIsWrong()
        {
            // Arrange
            var loginRequest = new LoginRequest("", "password123");
            var mockService = new Mock<IAuthenticationService>();
            var mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            var mockSignInManager = new Mock<SignInManager<User>>(mockUserManager.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<User>>(), null, null, null, null);
            mockUserManager.Setup(um => um.FindByEmailAsync(loginRequest.email))
                           .ReturnsAsync((User?)null);
            var sut = new AuthentificationController(mockService.Object, mockUserManager.Object, mockSignInManager.Object);

            // Act
            var result = await sut.Login(loginRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
        }

        [TestMethod]
        public async Task Login_ReturnUnauthorized_WhenPasswordIsWrong()
        {
            // Arrange
            var loginRequest = new LoginRequest("admin@example.com", "");
            var mockService = new Mock<IAuthenticationService>();
            var mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            var mockSignInManager = new Mock<SignInManager<User>>(mockUserManager.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<User>>(), null, null, null, null);
            mockUserManager.Setup(um => um.FindByEmailAsync(loginRequest.email))
                           .ReturnsAsync(new User { Email = loginRequest.email });
            mockSignInManager.Setup(sm => sm.CheckPasswordSignInAsync(It.IsAny<User>(), loginRequest.password, false))
                             .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);
            var sut = new AuthentificationController(mockService.Object, mockUserManager.Object, mockSignInManager.Object);
            
            // Act
            var result = await sut.Login(loginRequest);
            
            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
        }

        [TestMethod]
        public async Task Login_ReturnOk_WhenCredentialsAreValid()
        {
            // Arrange
            var loginRequest = new LoginRequest("admin@example.com", "password123");
            var mockService = new Mock<IAuthenticationService>();
            var mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            var mockSignInManager = new Mock<SignInManager<User>>(mockUserManager.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<User>>(), null, null, null, null);
            var user = new User { Email = loginRequest.email };
            mockUserManager.Setup(um => um.FindByEmailAsync(loginRequest.email))
                           .ReturnsAsync(user);
            mockSignInManager.Setup(sm => sm.CheckPasswordSignInAsync(user, loginRequest.password, false))
                             .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
            mockUserManager.Setup(um => um.GetRolesAsync(user))
                           .ReturnsAsync(new List<string> { "Admin" });
            mockService.Setup(s => s.GenerateJwtToken(user, It.IsAny<List<string>>()))
                       .Returns(("mocked_jwt_token", DateTime.UtcNow.AddHours(1)));
            var sut = new AuthentificationController(mockService.Object, mockUserManager.Object, mockSignInManager.Object);
            
            // Act
            var result = await sut.Login(loginRequest);
            
            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
    }
}
