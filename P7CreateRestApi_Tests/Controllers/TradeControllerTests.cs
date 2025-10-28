using Microsoft.AspNetCore.Mvc;
using Moq;
using P7CreateRestApi.Dto;
using P7CreateRestApi.Services;
using P7CreateRestApi.Controllers;

namespace P7CreateRestApi_Tests.Controllers
{
    [TestClass]
    public class TradeControllersTests
    {

        [TestMethod]
        public async Task ListAllTrades_ReturnOk_WhenListExist()
        {
            // Arrange
            var mockService = new Mock<ITradeService>();

            mockService.Setup(service => service.GetAllAsync())
                       .ReturnsAsync(new List<TradeDto>
                       {
                           new TradeDto { Id = 1, Account = "Account1", AccountType = "test" },
                           new TradeDto { Id = 2, Account = "Account2", AccountType = "test" }
                       });
            var sut = new TradeController(mockService.Object);

            // Act
            var result = await sut.ListAllTrades();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsNotNull((result as OkObjectResult)?.Value);
        }

        [TestMethod]
        public async Task ListAllTrades_ReturnNotFound_WhenListDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<ITradeService>();
            mockService.Setup(service => service.GetAllAsync())!
                       .ReturnsAsync((List<TradeDto>?)null);
            var sut = new TradeController(mockService.Object);

            // Act
            var result = await sut.ListAllTrades();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetTrade_ReturnOk_WhenTradeExists()
        {
            // Arrange
            var mockService = new Mock<ITradeService>();
            int tradeId = 1;
            mockService.Setup(service => service.GetByIdAsync(tradeId))
                       .ReturnsAsync(new TradeDto { Id = tradeId, Account = "Account1", AccountType = "test" });
            var sut = new TradeController(mockService.Object);

            // Act
            var result = await sut.GetTrade(tradeId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsNotNull((result as OkObjectResult)?.Value);
        }

        [TestMethod]
        public async Task GetTrade_ReturnNotFound_WhenTradeDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<ITradeService>();
            int tradeId = 1;
            mockService.Setup(service => service.GetByIdAsync(tradeId))
                       .ReturnsAsync((TradeDto?)null);
            var sut = new TradeController(mockService.Object);

            // Act
            var result = await sut.GetTrade(tradeId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task CreateTrade_ReturnCreatedAtActionResult_WhenTradeIsCreated()
        {
            // Arrange
            var mockService = new Mock<ITradeService>();
            var tradeDto = new TradeDto { Account = "Account1", AccountType = "test" };
            var createdId = 1;
            mockService.Setup(service => service.CreateAsync(tradeDto))
                       .ReturnsAsync(createdId);
            var sut = new TradeController(mockService.Object);

            // Act
            var result = await sut.CreateTrade(tradeDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            Assert.IsNotNull((result as CreatedAtActionResult)?.Value);
        }

        [TestMethod]
        public async Task UpdateTrade_ReturnNotFound_WhenTradeDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<ITradeService>();
            int tradeId = 1;
            var tradeDto = new TradeDto { Account = "Account1", AccountType = "test" };
            mockService.Setup(service => service.UpdateAsync(tradeId, tradeDto))
                       .ReturnsAsync((TradeDto?)null);
            var sut = new TradeController(mockService.Object);

            // Act
            var result = await sut.UpdateTrade(tradeId, tradeDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task UpdateTrade_ReturnOk_WhenTradeIsUpdated()
        {
            // Arrange
            var mockService = new Mock<ITradeService>();
            int tradeId = 1;
            var tradeDto = new TradeDto { Account = "Account1", AccountType = "test" };
            var updatedTradeDto = new TradeDto { Id = tradeId, Account = "Account1", AccountType = "test" };
            mockService.Setup(service => service.UpdateAsync(tradeId, tradeDto))
                       .ReturnsAsync(updatedTradeDto);
            var sut = new TradeController(mockService.Object);

            // Act
            var result = await sut.UpdateTrade(tradeId, tradeDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task DeleteTrade_ReturnNotFound_WhenTradeDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<ITradeService>();
            int tradeId = 1;
            mockService.Setup(service => service.DeleteAsync(tradeId))
                       .ReturnsAsync(false);
            var sut = new TradeController(mockService.Object);

            // Act
            var result = await sut.DeleteTrade(tradeId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task DeleteTrade_ReturnNoContent_WhenTradeIsDeleted()
        {
            // Arrange
            var mockService = new Mock<ITradeService>();
            int tradeId = 1;
            mockService.Setup(service => service.DeleteAsync(tradeId))
                       .ReturnsAsync(true);
            var sut = new TradeController(mockService.Object);

            // Act
            var result = await sut.DeleteTrade(tradeId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }
    }
}

