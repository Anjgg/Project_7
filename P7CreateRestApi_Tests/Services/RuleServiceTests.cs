using AutoMapper;
using Moq;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Dto;
using P7CreateRestApi.Repositories;
using P7CreateRestApi.Services;

namespace P7CreateRestApi_Tests.Services
{
    [TestClass]
    public class RuleServiceTests
    {
        [TestMethod]
        public async Task GetAllAsync_ReturnListOfRuleDto_WhenRulesAreFound()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Rule>>();
            var mockMapper = new Mock<IMapper>();
            var rules = new List<Rule>
            {
                new Rule { Id = 1, Name = "Rule1" },
                new Rule { Id = 2, Name = "Rule2" }
            };
            mockRepository.Setup(repo => repo.GetAllAsync())
                          .ReturnsAsync(rules);
            mockMapper.Setup(m => m.Map<RuleDto>(It.IsAny<Rule>()))
                      .Returns((Rule rule) => new RuleDto
                      {
                          Id = rule.Id,
                          Name = rule.Name
                      });
            var ruleService = new RuleService(mockRepository.Object, mockMapper.Object);
            // Act
            var result = await ruleService.GetAllAsync();
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, result[0].Id);
            Assert.AreEqual(2, result[1].Id);
        }

        [TestMethod]
        public async Task GetAllAsync_ReturnEmptyList_WhenNoRulesFound()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Rule>>();
            var mockMapper = new Mock<IMapper>();
            mockRepository.Setup(repo => repo.GetAllAsync())
                          .ReturnsAsync(new List<Rule>());
            var ruleService = new RuleService(mockRepository.Object, mockMapper.Object);

            // Act
            var result = await ruleService.GetAllAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }
        [TestMethod]
        public async Task GetByIdAsync_ReturnNull_WhenRuleDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Rule>>();
            var mockMapper = new Mock<IMapper>();
            int ruleId = 1;
            mockRepository.Setup(repo => repo.GetByIdAsync(ruleId))
                          .ReturnsAsync((Rule?)null);
            var ruleService = new RuleService(mockRepository.Object, mockMapper.Object);

            // Act
            var result = await ruleService.GetByIdAsync(ruleId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetByIdAsync_ReturnRuleDto_WhenRuleExists()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Rule>>();
            var mockMapper = new Mock<IMapper>();
            int ruleId = 1;
            var rule = new Rule { Id = ruleId, Name = "Test Rule" };
            mockRepository.Setup(repo => repo.GetByIdAsync(ruleId))
                          .ReturnsAsync(rule);
            mockMapper.Setup(m => m.Map<RuleDto>(It.IsAny<Rule>()))
                      .Returns((Rule r) => new RuleDto
                      {
                          Id = r.Id,
                          Name = r.Name
                      });
            var ruleService = new RuleService(mockRepository.Object, mockMapper.Object);

            // Act
            var result = await ruleService.GetByIdAsync(ruleId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(ruleId, result!.Id);
            Assert.AreEqual("Test Rule", result.Name);
        }

        [TestMethod]
        public async Task CreateAsync_ReturnCreatedRuleId_WhenRuleIsCreated()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Rule>>();
            var mockMapper = new Mock<IMapper>();
            var ruleDto = new RuleDto { Name = "New Rule" };
            var rule = new Rule { Id = 1, Name = "New Rule" };
            mockMapper.Setup(m => m.Map<Rule>(It.IsAny<RuleDto>()))
                      .Returns(rule);
            mockRepository.Setup(repo => repo.AddAsync(rule))
                          .ReturnsAsync(rule);
            var ruleService = new RuleService(mockRepository.Object, mockMapper.Object);

            // Act
            var result = await ruleService.CreateAsync(ruleDto);

            // Assert
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public async Task UpdateAsync_ReturnRuleDto_WhenRuleExist()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Rule>>();
            var mockMapper = new Mock<IMapper>();
            int ruleId = 1;
            var existingRule = new Rule { Id = ruleId, Description = "test" };
            var ruleDto = new RuleDto { Description = "updatedTest" };
            var updatedRule = new Rule { Id = ruleId, Description = "updatedTest" };
            mockRepository.Setup(repo => repo.GetByIdAsync(ruleId))
                          .ReturnsAsync(existingRule);
            mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Rule>()))
                          .ReturnsAsync(updatedRule);
            mockMapper.Setup(m => m.Map<RuleDto>(It.IsAny<Rule>()))
                      .Returns((Rule r) => new RuleDto
                      {
                          Id = r.Id,
                          Description = r.Description
                      });

            var ruleService = new RuleService(mockRepository.Object, mockMapper.Object);

            // Act
            var result = await ruleService.UpdateAsync(ruleId, ruleDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(ruleId, result!.Id);
            Assert.AreEqual("updatedTest", result.Description);
        }

        [TestMethod]
        public async Task UpdateAsync_ReturnNull_WhenRuleDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Rule>>();
            var mockMapper = new Mock<IMapper>();
            int ruleId = 1;
            var ruleDto = new RuleDto { Description = "updatedTest" };
            mockRepository.Setup(repo => repo.GetByIdAsync(ruleId))
                          .ReturnsAsync((Rule?)null);
            var ruleService = new RuleService(mockRepository.Object, mockMapper.Object);

            // Act
            var result = await ruleService.UpdateAsync(ruleId, ruleDto);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task DeleteAsync_ReturnTrue_WhenRuleIsDeleted()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Rule>>();
            int ruleId = 1;
            var existingRule = new Rule { Id = ruleId, Description = "test" };
            mockRepository.Setup(repo => repo.GetByIdAsync(ruleId))
                          .ReturnsAsync(existingRule);
            var ruleService = new RuleService(mockRepository.Object, Mock.Of<IMapper>());

            // Act
            var result = await ruleService.DeleteAsync(ruleId);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task DeleteAsync_ReturnFalse_WhenRuleDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Rule>>();
            int ruleId = 1;
            mockRepository.Setup(repo => repo.GetByIdAsync(ruleId))
                          .ReturnsAsync((Rule?)null);
            var ruleService = new RuleService(mockRepository.Object, Mock.Of<IMapper>());

            // Act
            var result = await ruleService.DeleteAsync(ruleId);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
