using AutoMapper;
using Moq;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Dto;
using P7CreateRestApi.Repositories;
using P7CreateRestApi.Services;

namespace P7CreateRestApi_Tests.Services
{
    [TestClass]
    public class TradeServiceTests
    {
        [TestMethod]
        public async Task GetAllAsync_ReturnListTradeDto_WhenTradeIsFound()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Trade>>();
            var mockMapper = new Mock<IMapper>();
            var trades = new List<Trade>
            {
                new Trade { Id = 1, Account = "trade1" },
                new Trade { Id = 2, Account = "trade2" }
            };
            mockRepository.Setup(repo => repo.GetAllAsync())
                          .ReturnsAsync(trades);
            mockMapper.Setup(m => m.Map<TradeDto>(It.IsAny<Trade>()))
                      .Returns((Trade trade) => new TradeDto { Id = trade.Id, Account = trade.Account });
            var tradeService = new TradeService(mockRepository.Object, mockMapper.Object);
            // Act
            var result = await tradeService.GetAllAsync();
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, result[0].Id);
            Assert.AreEqual(2, result[1].Id);
        }

        [TestMethod]
        public async Task GetAllAsync_ReturnEmptyList_WhenNoTradeFound()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Trade>>();
            var mockMapper = new Mock<IMapper>();
            mockRepository.Setup(repo => repo.GetAllAsync())
                          .ReturnsAsync(new List<Trade>());
            var tradeService = new TradeService(mockRepository.Object, mockMapper.Object);

            // Act
            var result = await tradeService.GetAllAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task GetByIdAsync_ReturnNull_WhenTradeDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Trade>>();
            var mockMapper = new Mock<IMapper>();
            mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                          .ReturnsAsync((Trade?)null);
            var tradeService = new TradeService(mockRepository.Object, mockMapper.Object);

            // Act
            var result = await tradeService.GetByIdAsync(1);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetByIdAsync_ReturnTradeDto_WhenTradeIsFound()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Trade>>();
            var mockMapper = new Mock<IMapper>();
            var trade = new Trade { Id = 1, Account = "trade1" };
            mockRepository.Setup(repo => repo.GetByIdAsync(1))
                          .ReturnsAsync(trade);
            mockMapper.Setup(m => m.Map<TradeDto>(It.IsAny<Trade>()))
                        .Returns((Trade t) => new TradeDto { Id = t.Id, Account = t.Account });
            var tradeService = new TradeService(mockRepository.Object, mockMapper.Object);

            // Act
            var result = await tradeService.GetByIdAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result!.Id);
            Assert.AreEqual("trade1", result.Account);
        }

        [TestMethod]
        public async Task CreateAsync_ReturnCreatedId_WhenTradeIsCreated()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Trade>>();
            var mockMapper = new Mock<IMapper>();
            var tradeDto = new TradeDto { Account = "trade1" };
            var trade = new Trade { Id = 1, Account = "trade1" };
            mockMapper.Setup(m => m.Map<Trade>(It.IsAny<TradeDto>()))
                      .Returns(trade);
            mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Trade>()))
                          .ReturnsAsync(trade);
            var tradeService = new TradeService(mockRepository.Object, mockMapper.Object);

            // Act
            var result = await tradeService.CreateAsync(tradeDto);

            // Assert
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public async Task UpdateAsync_ReturnTradeDto_WhenTradeExists()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Trade>>();
            var mockMapper = new Mock<IMapper>();
            int tradeId = 1;
            var existingTrade = new Trade { Id = tradeId, Account = "test" };
            var tradeDto = new TradeDto { Account = "updatedTest" };
            var updatedTrade = new Trade { Id = tradeId, Account = "updatedTest" };
            mockRepository.Setup(repo => repo.GetByIdAsync(tradeId))
                          .ReturnsAsync(existingTrade);
            mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Trade>()))
                          .ReturnsAsync(updatedTrade);
            mockMapper.Setup(m => m.Map<TradeDto>(It.IsAny<Trade>()))
                      .Returns((Trade r) => new TradeDto
                      {
                          Id = r.Id,
                          Account = r.Account
                      });

            var tradeService = new TradeService(mockRepository.Object, mockMapper.Object);

            // Act
            var result = await tradeService.UpdateAsync(tradeId, tradeDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(tradeId, result!.Id);
            Assert.AreEqual("updatedTest", result.Account);
        }

        [TestMethod]
        public async Task UpdateAsync_ReturnNull_WhenTradeDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Trade>>();
            var mockMapper = new Mock<IMapper>();
            int tradeId = 1;
            var tradeDto = new TradeDto { Account = "updatedTest" };
            mockRepository.Setup(repo => repo.GetByIdAsync(tradeId))
                          .ReturnsAsync((Trade?)null);
            var tradeService = new TradeService(mockRepository.Object, mockMapper.Object);

            // Act
            var result = await tradeService.UpdateAsync(tradeId, tradeDto);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task DeleteAsync_ReturnTrue_WhenTradeExists()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Trade>>();
            var mockMapper = new Mock<IMapper>();
            int tradeId = 1;
            var existingTrade = new Trade { Id = tradeId, Account = "test" };
            mockRepository.Setup(repo => repo.GetByIdAsync(tradeId))
                          .ReturnsAsync(existingTrade);
            var tradeService = new TradeService(mockRepository.Object, mockMapper.Object);

            // Act
            var result = await tradeService.DeleteAsync(tradeId);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task DeleteAsync_ReturnFalse_WhenTradeDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Trade>>();
            var mockMapper = new Mock<IMapper>();
            int tradeId = 1;
            mockRepository.Setup(repo => repo.GetByIdAsync(tradeId))
                          .ReturnsAsync((Trade?)null);
            var tradeService = new TradeService(mockRepository.Object, mockMapper.Object);

            // Act
            var result = await tradeService.DeleteAsync(tradeId);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
