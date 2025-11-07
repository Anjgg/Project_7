using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Dto;
using P7CreateRestApi.Services;

namespace P7CreateRestApi_Tests.Services
{
    [TestClass]
    public class UserServiceTests
    {
        private Mock<ILogger<UserService>> _mockLogger => new Mock<ILogger<UserService>>();

        [TestMethod]
        public async Task CreateAsync_ShouldReturnSuccessAndAddRole_WhenUserIsCreatedSuccessfully()
        {
            // Arrange
            var userDto = new CreateUserDto("test@test.com", "test1234", "DefaultUser");

            var userStoreMock = new Mock<IUserStore<User>>();
            var mockUserManager = new Mock<UserManager<User>>(userStoreMock.Object,
                                                              new Mock<IOptions<IdentityOptions>>().Object,
                                                              new Mock<IPasswordHasher<User>>().Object,
                                                              new IUserValidator<User>[0],
                                                              new IPasswordValidator<User>[0],
                                                              new Mock<ILookupNormalizer>().Object,
                                                              new IdentityErrorDescriber(),
                                                              null,
                                                              new Mock<ILogger<UserManager<User>>>().Object);

            mockUserManager.Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                           .ReturnsAsync(IdentityResult.Success);
            mockUserManager.Setup(um => um.IsInRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                           .ReturnsAsync(false);
            mockUserManager.Setup(um => um.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                           .ReturnsAsync(IdentityResult.Success);

            var sut = new UserService(mockUserManager.Object, _mockLogger.Object);

            // Act
            var (isSuccess, errors) = await sut.CreateAsync(userDto);

            // Assert
            Assert.IsTrue(isSuccess);
            Assert.IsNull(errors);
        }

        [TestMethod]
        public async Task CreateAsync_ShouldReturnFailure_WhenUserCreationFails()
        {
            // Arrange
            var userDto = new CreateUserDto("test@test.com", "test1234", "DefaultUser");
            var userStoreMock = new Mock<IUserStore<User>>();
            var mockUserManager = new Mock<UserManager<User>>(userStoreMock.Object,
                                                              new Mock<IOptions<IdentityOptions>>().Object,
                                                              new Mock<IPasswordHasher<User>>().Object,
                                                              new IUserValidator<User>[0],
                                                              new IPasswordValidator<User>[0],
                                                              new Mock<ILookupNormalizer>().Object,
                                                              new IdentityErrorDescriber(),
                                                              null,
                                                              new Mock<ILogger<UserManager<User>>>().Object);
            mockUserManager.Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                           .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "User creation failed" }));
            var sut = new UserService(mockUserManager.Object, _mockLogger.Object);

            // Act
            var (isSuccess, errors) = await sut.CreateAsync(userDto);

            // Assert
            Assert.IsFalse(isSuccess);
            Assert.IsNotNull(errors);
        }

        [TestMethod]
        public async Task DeleteAsync_ShouldReturnSuccess_WhenUserIsDeletedSuccessfully()
        {
            // Arrange
            string userId = "1";
            var userStoreMock = new Mock<IUserStore<User>>();
            var mockUserManager = new Mock<UserManager<User>>(userStoreMock.Object,
                                                              new Mock<IOptions<IdentityOptions>>().Object,
                                                              new Mock<IPasswordHasher<User>>().Object,
                                                              new IUserValidator<User>[0],
                                                              new IPasswordValidator<User>[0],
                                                              new Mock<ILookupNormalizer>().Object,
                                                              new IdentityErrorDescriber(),
                                                              null,
                                                              new Mock<ILogger<UserManager<User>>>().Object);
            mockUserManager.Setup(um => um.FindByIdAsync(userId))
                           .ReturnsAsync(new User { Id = userId });
            mockUserManager.Setup(um => um.DeleteAsync(It.IsAny<User>()))
                           .ReturnsAsync(IdentityResult.Success);
            var sut = new UserService(mockUserManager.Object, _mockLogger.Object);

            // Act
            var (isSuccess, errors) = await sut.DeleteAsync(userId);

            // Assert
            Assert.IsTrue(isSuccess);
            Assert.IsNull(errors);
        }

        [TestMethod]
        public async Task DeleteAsync_ShouldReturnFailure_WhenUserDoesNotExist()
        {
            // Arrange
            string userId = "1";
            var userStoreMock = new Mock<IUserStore<User>>();
            var mockUserManager = new Mock<UserManager<User>>(userStoreMock.Object,
                                                              new Mock<IOptions<IdentityOptions>>().Object,
                                                              new Mock<IPasswordHasher<User>>().Object,
                                                              new IUserValidator<User>[0],
                                                              new IPasswordValidator<User>[0],
                                                              new Mock<ILookupNormalizer>().Object,
                                                              new IdentityErrorDescriber(),
                                                              null,
                                                              new Mock<ILogger<UserManager<User>>>().Object);
            mockUserManager.Setup(um => um.FindByIdAsync(userId))
                           .ReturnsAsync((User?)null);
            var sut = new UserService(mockUserManager.Object, _mockLogger.Object);
            // Act
            var (isSuccess, errors) = await sut.DeleteAsync(userId);
            // Assert
            Assert.IsFalse(isSuccess);
            Assert.IsNotNull(errors);
            Assert.AreEqual("User not found", errors?.First());
        }

        [TestMethod]
        public async Task DeleteAsync_ShouldReturnFailure_WhenDeletionFails()
        {
            // Arrange
            string userId = "1";
            var userStoreMock = new Mock<IUserStore<User>>();
            var mockUserManager = new Mock<UserManager<User>>(userStoreMock.Object,
                                                              new Mock<IOptions<IdentityOptions>>().Object,
                                                              new Mock<IPasswordHasher<User>>().Object,
                                                              new IUserValidator<User>[0],
                                                              new IPasswordValidator<User>[0],
                                                              new Mock<ILookupNormalizer>().Object,
                                                              new IdentityErrorDescriber(),
                                                              null,
                                                              new Mock<ILogger<UserManager<User>>>().Object);
            mockUserManager.Setup(um => um.FindByIdAsync(userId))
                           .ReturnsAsync(new User { Id = userId });
            mockUserManager.Setup(um => um.DeleteAsync(It.IsAny<User>()))
                           .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Deletion failed" }));
            var sut = new UserService(mockUserManager.Object, _mockLogger.Object);

            // Act
            var (isSuccess, errors) = await sut.DeleteAsync(userId);

            // Assert
            Assert.IsFalse(isSuccess);
            Assert.IsNotNull(errors);
        }

        [TestMethod]
        public void GetAllUsers_ShouldReturnListOfUserDtos()
        {
            // Arrange
            var userStoreMock = new Mock<IUserStore<User>>();
            var mockUserManager = new Mock<UserManager<User>>(userStoreMock.Object,
                                                              new Mock<IOptions<IdentityOptions>>().Object,
                                                              new Mock<IPasswordHasher<User>>().Object,
                                                              new IUserValidator<User>[0],
                                                              new IPasswordValidator<User>[0],
                                                              new Mock<ILookupNormalizer>().Object,
                                                              new IdentityErrorDescriber(),
                                                              null,
                                                              new Mock<ILogger<UserManager<User>>>().Object);
            mockUserManager.Setup(um => um.Users)
                           .Returns(new List<User>
                           {
                               new User { Id = "1", Email = "user1@test.com" },
                               new User { Id = "2", Email = "user2@test.com" }
                           }.AsQueryable());
            mockUserManager.Setup(um => um.GetRolesAsync(It.IsAny<User>()))
                           .ReturnsAsync(new List<string> { "DefaultUser" });

            var sut = new UserService(mockUserManager.Object, _mockLogger.Object);

            // Act
            var users = sut.GetAllUsers();

            // Assert
            Assert.IsNotNull(users);
            Assert.AreEqual(2, users?.Count);
        }

        [TestMethod]
        public async Task GetByIdAsync_ShouldReturnUserDto_WhenUserExists()
        {
            // Arrange
            string userId = "1";
            var userStoreMock = new Mock<IUserStore<User>>();
            var mockUserManager = new Mock<UserManager<User>>(userStoreMock.Object,
                                                              new Mock<IOptions<IdentityOptions>>().Object,
                                                              new Mock<IPasswordHasher<User>>().Object,
                                                              new IUserValidator<User>[0],
                                                              new IPasswordValidator<User>[0],
                                                              new Mock<ILookupNormalizer>().Object,
                                                              new IdentityErrorDescriber(),
                                                              null,
                                                              new Mock<ILogger<UserManager<User>>>().Object);
            mockUserManager.Setup(um => um.FindByIdAsync(userId))
                           .ReturnsAsync(new User { Id = userId, Email = "user@test.com" });
            mockUserManager.Setup(um => um.GetRolesAsync(It.IsAny<User>()))
                            .ReturnsAsync(new List<string> { "DefaultUser" });
            var sut = new UserService(mockUserManager.Object, _mockLogger.Object);

            // Act
            var userDto = await sut.GetByIdAsync(userId);

            // Assert
            Assert.IsNotNull(userDto);
            Assert.AreEqual(userId, userDto?.id);
        }

        [TestMethod]
        public async Task GetByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            string userId = "1";
            var userStoreMock = new Mock<IUserStore<User>>();
            var mockUserManager = new Mock<UserManager<User>>(userStoreMock.Object,
                                                              new Mock<IOptions<IdentityOptions>>().Object,
                                                              new Mock<IPasswordHasher<User>>().Object,
                                                              new IUserValidator<User>[0],
                                                              new IPasswordValidator<User>[0],
                                                              new Mock<ILookupNormalizer>().Object,
                                                              new IdentityErrorDescriber(),
                                                              null,
                                                              new Mock<ILogger<UserManager<User>>>().Object);
            mockUserManager.Setup(um => um.FindByIdAsync(userId))
                           .ReturnsAsync((User?)null);
            var sut = new UserService(mockUserManager.Object, _mockLogger.Object);

            // Act
            var userDto = await sut.GetByIdAsync(userId);

            // Assert
            Assert.IsNull(userDto);
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldReturnSuccess_WhenUserIsUpdatedSuccessfully()
        {
            // Arrange
            string userId = "1";
            var updateUserDto = new UpdateUserDto("test@test.com", "newpassword123", "DefaultUser");
            var userStoreMock = new Mock<IUserStore<User>>();
            var mockUserManager = new Mock<UserManager<User>>(userStoreMock.Object,
                                                              new Mock<IOptions<IdentityOptions>>().Object,
                                                              new Mock<IPasswordHasher<User>>().Object,
                                                              new IUserValidator<User>[0],
                                                              new IPasswordValidator<User>[0],
                                                              new Mock<ILookupNormalizer>().Object,
                                                              new IdentityErrorDescriber(),
                                                              null,
                                                              new Mock<ILogger<UserManager<User>>>().Object);

            mockUserManager.Setup(um => um.FindByIdAsync(userId))
                           .ReturnsAsync(new User { Id = userId, Email = "test@test.com" });
            mockUserManager.Setup(um => um.GeneratePasswordResetTokenAsync(It.IsAny<User>()))
                           .ReturnsAsync("resettoken");
            mockUserManager.Setup(um => um.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                           .ReturnsAsync(IdentityResult.Success);
            mockUserManager.Setup(um => um.GetRolesAsync(It.IsAny<User>()))
                           .ReturnsAsync(new List<string> { "OldRole" });
            mockUserManager.Setup(um => um.RemoveFromRolesAsync(It.IsAny<User>(), It.IsAny<IEnumerable<string>>()))
                           .ReturnsAsync(IdentityResult.Success);
            mockUserManager.Setup(um => um.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                           .ReturnsAsync(IdentityResult.Success);

            var sut = new UserService(mockUserManager.Object, _mockLogger.Object);

            // Act
            var (isSuccess, errors) = await sut.UpdateAsync(userId, updateUserDto);

            // Assert
            Assert.IsTrue(isSuccess);

        }

        [TestMethod]
        public async Task UpdateAsync_ShouldReturnFailure_WhenUserDoesNotExist()
        {
            // Arrange
            string userId = "1";
            var updateUserDto = new UpdateUserDto("test@test.com", "newpassword123", "DefaultUser");
            var userStoreMock = new Mock<IUserStore<User>>();
            var mockUserManager = new Mock<UserManager<User>>(userStoreMock.Object,
                                                              new Mock<IOptions<IdentityOptions>>().Object,
                                                              new Mock<IPasswordHasher<User>>().Object,
                                                              new IUserValidator<User>[0],
                                                              new IPasswordValidator<User>[0],
                                                              new Mock<ILookupNormalizer>().Object,
                                                              new IdentityErrorDescriber(),
                                                              null,
                                                              new Mock<ILogger<UserManager<User>>>().Object);
            mockUserManager.Setup(um => um.FindByIdAsync(userId))
                            .ReturnsAsync((User?)null);

            var sut = new UserService(mockUserManager.Object, _mockLogger.Object);

            // Act
            var (isSuccess, errors) = await sut.UpdateAsync(userId, updateUserDto);

            // Assert
            Assert.IsFalse(isSuccess);
            Assert.IsNotNull(errors);

        }

        [TestMethod]
        public async Task UpdateAsync_ShouldReturnFailure_WhenPasswordResetFails()
        {
            // Arrange
            string userId = "1";
            var updateUserDto = new UpdateUserDto("test@test.com", "newpassword123", "DefaultUser");
            var userStoreMock = new Mock<IUserStore<User>>();
            var mockUserManager = new Mock<UserManager<User>>(userStoreMock.Object,
                                                              new Mock<IOptions<IdentityOptions>>().Object,
                                                              new Mock<IPasswordHasher<User>>().Object,
                                                              new IUserValidator<User>[0],
                                                              new IPasswordValidator<User>[0],
                                                              new Mock<ILookupNormalizer>().Object,
                                                              new IdentityErrorDescriber(),
                                                              null,
                                                              new Mock<ILogger<UserManager<User>>>().Object);
            mockUserManager.Setup(um => um.FindByIdAsync(userId))
                            .ReturnsAsync(new User { Id = userId, Email = "test@test.com" });
            mockUserManager.Setup(um => um.GeneratePasswordResetTokenAsync(It.IsAny<User>()))
                            .ReturnsAsync("resettoken");
            mockUserManager.Setup(um => um.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Password reset failed" }));

            var sut = new UserService(mockUserManager.Object, _mockLogger.Object);
            // Act
            var (isSuccess, errors) = await sut.UpdateAsync(userId, updateUserDto);

            // Assert
            Assert.IsFalse(isSuccess);
            Assert.IsNotNull(errors);
        }
    }
}