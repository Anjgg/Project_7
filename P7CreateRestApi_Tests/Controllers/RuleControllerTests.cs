using Microsoft.AspNetCore.Mvc;
using Moq;
using P7CreateRestApi.Dto;
using P7CreateRestApi.Services;
using P7CreateRestApi.Controllers;

namespace P7CreateRestApi_Tests.Controllers
{
    [TestClass]
    public class RuleControllerTests
    {

        [TestMethod]
        public async Task ListAllRules_ReturnOk_WhenListExist()
        {
            // Arrange
            var mockService = new Mock<IRuleService>();

            mockService.Setup(service => service.GetAllAsync())
                       .ReturnsAsync(new List<RuleDto>
                       {
                           new RuleDto { Id = 1, Description = "test", Json = "test", Name = "test", SqlPart = "test", SqlStr = "test", Template = "test"},
                           new RuleDto { Id = 2, Description = "test", Json = "test", Name = "test", SqlPart = "test", SqlStr = "test", Template = "test"}
                       });
            var sut = new RuleController(mockService.Object);

            // Act
            var result = await sut.ListAllRules();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsNotNull((result as OkObjectResult)?.Value);
        }

        [TestMethod]
        public async Task ListAllRules_ReturnNotFound_WhenListDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<IRuleService>();
            mockService.Setup(service => service.GetAllAsync())!
                       .ReturnsAsync((List<RuleDto>?)null);
            var sut = new RuleController(mockService.Object);

            // Act
            var result = await sut.ListAllRules();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetRule_ReturnOk_WhenRuleExists()
        {
            // Arrange
            var mockService = new Mock<IRuleService>();
            int ruleId = 1;
            mockService.Setup(service => service.GetByIdAsync(ruleId))
                       .ReturnsAsync(new RuleDto { Description = "test", Json = "test", Name = "test", SqlPart = "test", SqlStr = "test", Template = "test" });
            var sut = new RuleController(mockService.Object);

            // Act
            var result = await sut.GetRule(ruleId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsNotNull((result as OkObjectResult)?.Value);
        }

        [TestMethod]
        public async Task GetRule_ReturnNotFound_WhenRuleDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<IRuleService>();
            int ruleId = 1;
            mockService.Setup(service => service.GetByIdAsync(ruleId))
                       .ReturnsAsync((RuleDto?)null);
            var sut = new RuleController(mockService.Object);

            // Act
            var result = await sut.GetRule(ruleId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task CreateRule_ReturnCreatedAtActionResult_WhenRuleIsCreated()
        {
            // Arrange
            var mockService = new Mock<IRuleService>();
            var ruleDto = new RuleDto { Description = "test", Json = "test", Name = "test", SqlPart = "test", SqlStr = "test", Template = "test" };
            var createdId = 1;
            mockService.Setup(service => service.CreateAsync(ruleDto))
                       .ReturnsAsync(createdId);
            var sut = new RuleController(mockService.Object);

            // Act
            var result = await sut.CreateRule(ruleDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            Assert.IsNotNull((result as CreatedAtActionResult)?.Value);
        }

        [TestMethod]
        public async Task UpdateRule_ReturnNotFound_WhenRuleDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<IRuleService>();
            int ruleId = 1;
            var ruleDto = new RuleDto { Description = "test", Json = "test", Name = "test", SqlPart = "test", SqlStr = "test", Template = "test" };
            mockService.Setup(service => service.UpdateAsync(ruleId, ruleDto))
                       .ReturnsAsync((RuleDto?)null);
            var sut = new RuleController(mockService.Object);

            // Act
            var result = await sut.UpdateRule(ruleId, ruleDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task UpdateRule_ReturnOk_WhenRuleIsUpdated()
        {
            // Arrange
            var mockService = new Mock<IRuleService>();
            int ruleId = 1;
            var ruleDto = new RuleDto { Description = "test", Json = "test", Name = "test", SqlPart = "test", SqlStr = "test", Template = "test" };
            var updatedRuleDto = new RuleDto { Id = ruleId, Description = "test", Json = "test", Name = "test", SqlPart = "test", SqlStr = "test", Template = "test" };
            mockService.Setup(service => service.UpdateAsync(ruleId, ruleDto))
                       .ReturnsAsync(updatedRuleDto);
            var sut = new RuleController(mockService.Object);

            // Act
            var result = await sut.UpdateRule(ruleId, ruleDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task DeleteRule_ReturnNotFound_WhenRuleDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<IRuleService>();
            int ruleId = 1;
            mockService.Setup(service => service.DeleteAsync(ruleId))
                       .ReturnsAsync(false);
            var sut = new RuleController(mockService.Object);

            // Act
            var result = await sut.DeleteRule(ruleId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task DeleteRule_ReturnNoContent_WhenRuleIsDeleted()
        {
            // Arrange
            var mockService = new Mock<IRuleService>();
            int ruleId = 1;
            mockService.Setup(service => service.DeleteAsync(ruleId))
                       .ReturnsAsync(true);
            var sut = new RuleController(mockService.Object);

            // Act
            var result = await sut.DeleteRule(ruleId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }
    }
}


