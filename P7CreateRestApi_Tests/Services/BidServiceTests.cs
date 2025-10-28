using AutoMapper;
using Moq;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Dto;
using P7CreateRestApi.Repositories;
using P7CreateRestApi.Services;

namespace P7CreateRestApi_Tests.Services
{
    [TestClass]
    public class BidServiceTests
    {
        [TestMethod]
        public async Task GetAllAsync_ReturnListBidDto_WhenBidIsFound()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Bid>>();
            var mockMapper = new Mock<IMapper>();
            var bids = new List<Bid>
            {
                new Bid { Id = 1, Account = "test1", Type = "test" },
                new Bid { Id = 3, Account = "test2", Type = "test" }
            };
            mockRepository.Setup(repo => repo.GetAllAsync())
                          .ReturnsAsync(bids);
            mockMapper.Setup(m => m.Map<BidDto>(It.IsAny<Bid>()))
                      .Returns((Bid bid) => new BidDto { Id = bid.Id, Account = bid.Account, Type = bid.Type });
            var bidService = new BidService(mockRepository.Object, mockMapper.Object);
            // Act
            var result = await bidService.GetAllAsync();
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, result[0].Id);
            Assert.AreEqual(3, result[1].Id);
        }

        [TestMethod]
        public async Task GetAllAsync_ReturnEmptyList_WhenNoBidFound()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Bid>>();
            var mockMapper = new Mock<IMapper>();
            mockRepository.Setup(repo => repo.GetAllAsync())
                          .ReturnsAsync(new List<Bid>());
            var bidService = new BidService(mockRepository.Object, mockMapper.Object);

            // Act
            var result = await bidService.GetAllAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task GetByIdAsync_ReturnNull_WhenBidDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Bid>>();
            var mockMapper = new Mock<IMapper>();
            int bidId = 1;
            mockRepository.Setup(repo => repo.GetByIdAsync(bidId))
                          .ReturnsAsync((Bid?)null);
            var bidService = new BidService(mockRepository.Object, mockMapper.Object);
            // Act
            var result = await bidService.GetByIdAsync(bidId);
            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetByIdAsync_ReturnBidDto_WhenBidExists()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Bid>>();
            var mockMapper = new Mock<IMapper>();
            int bidId = 1;
            var bid = new Bid { Id = bidId, Account = "test1", Type = "test" };
            mockRepository.Setup(repo => repo.GetByIdAsync(bidId))
                          .ReturnsAsync(bid);
            mockMapper.Setup(m => m.Map<BidDto>(It.IsAny<Bid>()))
                      .Returns((Bid b) => new BidDto { Id = b.Id, Account = b.Account, Type = b.Type });
            var bidService = new BidService(mockRepository.Object, mockMapper.Object);

            // Act
            var result = await bidService.GetByIdAsync(bidId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(bidId, result?.Id);
            Assert.AreEqual("test1", result?.Account);
            Assert.AreEqual("test", result?.Type);
        }

        [TestMethod]
        public async Task CreateAsync_ReturnCreatedBidId()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Bid>>();
            var mockMapper = new Mock<IMapper>();
            var bidDto = new BidDto { Account = "test1", Type = "test" };
            var id = new Random().Next(1,10);
            var bid = new Bid { Id = id, Account = "test1", Type = "test" };
            mockMapper.Setup(m => m.Map<Bid>(It.IsAny<BidDto>()))
                      .Returns(bid);
            mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Bid>()))
                          .ReturnsAsync(bid);
            var bidService = new BidService(mockRepository.Object, mockMapper.Object);

            // Act
            var result = await bidService.CreateAsync(bidDto);

            // Assert
            Assert.AreEqual(id, result);
        }

        [TestMethod]
        public async Task UpdateAsync_ReturnNull_WhenBidDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Bid>>();
            var mockMapper = new Mock<IMapper>();
            int bidId = 1;
            var bidDto = new BidDto { Account = "updatedAccount", Type = "updatedType" };
            mockRepository.Setup(repo => repo.GetByIdAsync(bidId))
                          .ReturnsAsync((Bid?)null);
            var bidService = new BidService(mockRepository.Object, mockMapper.Object);

            // Act
            var result = await bidService.UpdateAsync(bidId, bidDto);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task UpdateAsync_ReturnUpdatedBidDto_WhenBidExists()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Bid>>();
            var mockMapper = new Mock<IMapper>();
            int bidId = 1;
            var existingBid = new Bid { Id = bidId, Account = "test1", Type = "test" };
            var bidDto = new BidDto { Account = "updatedAccount", Type = "updatedType" };
            var updatedBid = new Bid { Id = bidId, Account = "updatedAccount", Type = "updatedType" };
            mockRepository.Setup(repo => repo.GetByIdAsync(bidId))
                          .ReturnsAsync(existingBid);
            mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Bid>()))
                          .ReturnsAsync(updatedBid);
            mockMapper.Setup(m => m.Map<BidDto>(It.IsAny<Bid>()))
                      .Returns((Bid b) => new BidDto { Id = b.Id, Account = b.Account, Type = b.Type });
            var bidService = new BidService(mockRepository.Object, mockMapper.Object);

            // Act
            var result = await bidService.UpdateAsync(bidId, bidDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(bidId, result?.Id);
            Assert.AreEqual("updatedAccount", result?.Account);
            Assert.AreEqual("updatedType", result?.Type);
        }

        [TestMethod]
        public async Task DeleteAsync_ReturnTrue_WhenBidExists()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Bid>>();
            var mockMapper = new Mock<IMapper>();
            int bidId = 1;
            var existingBid = new Bid { Id = bidId, Account = "test1", Type = "test" };
            mockRepository.Setup(repo => repo.GetByIdAsync(bidId))
                          .ReturnsAsync(existingBid);
            var bidService = new BidService(mockRepository.Object, mockMapper.Object);

            // Act
            var result = await bidService.DeleteAsync(bidId);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task DeleteAsync_ReturnFalse_WhenBidDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Bid>>();
            var mockMapper = new Mock<IMapper>();
            int bidId = 1;
            mockRepository.Setup(repo => repo.GetByIdAsync(bidId))
                          .ReturnsAsync((Bid?)null);
            var bidService = new BidService(mockRepository.Object, mockMapper.Object);
            // Act
            var result = await bidService.DeleteAsync(bidId);
            // Assert
            Assert.IsFalse(result);
        }
    }
}
