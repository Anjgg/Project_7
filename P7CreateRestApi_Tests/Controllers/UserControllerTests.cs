using Microsoft.AspNetCore.Mvc;
using Moq;
using P7CreateRestApi.Controllers;
using P7CreateRestApi.Dto;
using P7CreateRestApi.Services;

namespace P7CreateRestApi_Tests.Controllers
{
    [TestClass]
    public class UserControllerTests
    {
        // Assert
        [TestMethod]
        public void ListAllUsers_ReturnNotFound_WhenNoUsersFound()
        {
            // Arrange
            var mockService = new Mock<IUserService>();
            mockService.Setup(service => service.GetAllUsers())
                       .Returns((List<UserDto>?)null);
            var sut = new UserController(mockService.Object);

            // Act
            var result = sut.ListAllUsers();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}
