using Microsoft.Extensions.Options;
using Moq;
using P7CreateRestApi.Config;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Services;

namespace P7CreateRestApi_Tests.Services
{
    [TestClass]
    public class AuthentificationServiceTests
    {
        [TestMethod]
        public void GenerateJwtToken_ReturnsTokenString_WhenUserIsValid()
        {
            // Arrange
            var mockUserService = new Mock<IOptions<JwtOptions>>();
            mockUserService.Setup(opts => opts.Value).Returns(new JwtOptions
            {
                Key = "F5B1D22D9064FB449C893A48F14B92645FFAFE2E4ED5C1897C5E74A4FE37F4FBCCE0228604277F2C2FE0792079F14118CC98559824EAB15BBCE168E7AD61ACCC",
                Issuer = "testIssuer",
                Audience = "testAudience",
                ExpiryMinutes = 60
            });
            var user = new User
            {
                Id = "1",
                Email = "testuser@example.com",
            };
            var roles = new List<string> { "Admin", "DefaultUser" };
            var authentificationService = new AuthenticationService(mockUserService.Object);

            // Act
            var (token, expiration) = authentificationService.GenerateJwtToken(user, roles);
            // Assert
            Assert.IsNotNull(token);
            Assert.IsInstanceOfType(token, typeof(string));
            Assert.IsTrue(token.Length > 0);
        }

        [TestMethod]
        public void GenerateJwtToken_ShouldWorkWithNoRoles()
        {
            // Arrange
            var mockUserService = new Mock<IOptions<JwtOptions>>();
            mockUserService.Setup(opts => opts.Value).Returns(new JwtOptions
            {
                Key = "F5B1D22D9064FB449C893A48F14B92645FFAFE2E4ED5C1897C5E74A4FE37F4FBCCE0228604277F2C2FE0792079F14118CC98559824EAB15BBCE168E7AD61ACCC",
                Issuer = "testIssuer",
                Audience = "testAudience",
                ExpiryMinutes = 60
            });
            var user = new User
            {
                Id = "1",
                Email = "testuser@example.com"
            };
            var roles = new List<string>();
            var authentificationService = new AuthenticationService(mockUserService.Object);

            // Act
            var (token, expiration) = authentificationService.GenerateJwtToken(user, roles);

            // Assert
            Assert.IsNotNull(token);
            Assert.IsInstanceOfType(token, typeof(string));
            Assert.IsTrue(token.Length > 0);
        }
    }
}
