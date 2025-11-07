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

        [TestMethod]
        public void ListAllUsers_ReturnOk_WhenUsersIsFound()
        {
            // Arrange
            var mockService = new Mock<IUserService>();
            mockService.Setup(service => service.GetAllUsers())
                       .Returns(new List<UserDto>
                       {
                           new UserDto("1", "testuser1", new List<string> { "Admin" }),
                           new UserDto("2", "testuser2", new List<string> { "User" })
                       });
            var sut = new UserController(mockService.Object);
            // Act
            var result = sut.ListAllUsers();
            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetUser_ReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<IUserService>();
            string userId = "1";
            mockService.Setup(service => service.GetByIdAsync(userId))
                       .ReturnsAsync((UserDto?)null);
            var sut = new UserController(mockService.Object);

            // Act
            var result = await sut.GetUser(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetUser_ReturnOk_WhenUserExists()
        {
            // Arrange
            var mockService = new Mock<IUserService>();
            string userId = "1";
            mockService.Setup(service => service.GetByIdAsync(userId))
                       .ReturnsAsync(new UserDto(userId, "testuser", new List<string> { "Admin" }));
            var sut = new UserController(mockService.Object);

            // Act
            var result = await sut.GetUser(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task CreateUser_ReturnBadRequest_WhenCreationFails()
        {
            // Arrange
            var mockService = new Mock<IUserService>();
            var userDto = new CreateUserDto("testuser", "password123", "Admin");
            mockService.Setup(service => service.CreateAsync(userDto))
                       .ReturnsAsync((false, new List<string> { "Error creating user" }));
            var sut = new UserController(mockService.Object);
            // Act
            var result = await sut.CreateUser(userDto);
            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task CreateUser_ReturnCreatedAtAction_WhenCreationSucceeds()
        {
            // Arrange
            var mockService = new Mock<IUserService>();
            var userDto = new CreateUserDto("testuser", "password123", "Admin");
            mockService.Setup(service => service.CreateAsync(userDto))
                       .ReturnsAsync((true, new List<string>()));
            var sut = new UserController(mockService.Object);

            // Act
            var result = await sut.CreateUser(userDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        }

        [TestMethod]
        public async Task UpdateUser_ReturnNoContent_WhenUpdateSucceeds()
        {
            // Arrange
            var mockService = new Mock<IUserService>();
            string userId = "1";
            var updateUserDto = new UpdateUserDto("updatedUser", "NewPassword123", "User");
            mockService.Setup(service => service.UpdateAsync(userId, updateUserDto))
                       .ReturnsAsync((true, null));
            var sut = new UserController(mockService.Object);

            // Act
            var result = await sut.UpdateUser(userId, updateUserDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task UpdateUser_ReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<IUserService>();
            string userId = "1";
            var updateUserDto = new UpdateUserDto("updatedUser", "NewPassword123", "User");
            mockService.Setup(service => service.UpdateAsync(userId, updateUserDto))
                       .ReturnsAsync((false, new List<string> { "User not found" }));
            var sut = new UserController(mockService.Object);

            // Act
            var result = await sut.UpdateUser(userId, updateUserDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task UpdateUser_ReturnBadRequest_WhenUpdateFails()
        {
            // Arrange
            var mockService = new Mock<IUserService>();
            string userId = "1";
            var updateUserDto = new UpdateUserDto("updatedUser", "NewPassword123", "User");
            mockService.Setup(service => service.UpdateAsync(userId, updateUserDto))
                       .ReturnsAsync((false, new List<string> { "Error updating user" }));
            var sut = new UserController(mockService.Object);

            // Act
            var result = await sut.UpdateUser(userId, updateUserDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task DeleteUser_ReturnNoContent_WhenDeleteSucceeds()
        {
            // Arrange
            var mockService = new Mock<IUserService>();
            string userId = "1";
            mockService.Setup(service => service.DeleteAsync(userId))
                       .ReturnsAsync((true, null));
            var sut = new UserController(mockService.Object);

            // Act
            var result = await sut.DeleteUser(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task DeleteUser_ReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<IUserService>();
            string userId = "1";
            mockService.Setup(service => service.DeleteAsync(userId))
                       .ReturnsAsync((false, new List<string> { "User not found" }));
            var sut = new UserController(mockService.Object);

            // Act
            var result = await sut.DeleteUser(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task DeleteUser_ReturnBadRequest_WhenDeleteFails()
        {
            // Arrange
            var mockService = new Mock<IUserService>();
            string userId = "1";
            mockService.Setup(service => service.DeleteAsync(userId))
                       .ReturnsAsync((false, new List<string> { "Error deleting user" }));
            var sut = new UserController(mockService.Object);

            // Act
            var result = await sut.DeleteUser(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }
    }
}