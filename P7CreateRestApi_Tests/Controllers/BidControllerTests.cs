using Microsoft.AspNetCore.Mvc;
using Moq;
using P7CreateRestApi.Dto;
using P7CreateRestApi.Services;
using P7CreateRestApi.Controllers;

namespace P7CreateRestApi_Tests.Controllers
{
    [TestClass]
    public class BidControllersTests
    {

        [TestMethod]
        public async Task ListAllBids_ReturnOk_WhenListExist()
        {
            // Arrange
            var mockService = new Mock<IBidService>();

            mockService.Setup(service => service.GetAllAsync())
                       .ReturnsAsync(new List<BidDto>
                       {
                           new BidDto { Id = 1, Account = "Account1", Type = "Type1", BidQuantity = 10 },
                           new BidDto { Id = 2, Account = "Account2", Type = "Type2", BidQuantity = 20 }
                       });
            var sut = new BidController(mockService.Object);

            // Act
            var result = await sut.ListAllBids();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsNotNull((result as OkObjectResult)?.Value);
        }

        [TestMethod]
        public async Task ListAllBids_ReturnNotFound_WhenListDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<IBidService>();
            mockService.Setup(service => service.GetAllAsync())!
                       .ReturnsAsync((List<BidDto>?)null);
            var sut = new BidController(mockService.Object);

            // Act
            var result = await sut.ListAllBids();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetBid_ReturnOk_WhenBidExists()
        {
            // Arrange
            var mockService = new Mock<IBidService>();
            int bidId = 1;
            mockService.Setup(service => service.GetByIdAsync(bidId))
                       .ReturnsAsync(new BidDto { Id = bidId, Account = "Account1", Type = "Type1", BidQuantity = 10 });
            var sut = new BidController(mockService.Object);

            // Act
            var result = await sut.GetBid(bidId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsNotNull((result as OkObjectResult)?.Value);
        }

        [TestMethod]
        public async Task GetBid_ReturnNotFound_WhenBidDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<IBidService>();
            int bidId = 1;
            mockService.Setup(service => service.GetByIdAsync(bidId))
                       .ReturnsAsync((BidDto?)null);
            var sut = new BidController(mockService.Object);

            // Act
            var result = await sut.GetBid(bidId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task CreateBid_ReturnCreatedAtActionResult_WhenBidIsCreated()
        {
            // Arrange
            var mockService = new Mock<IBidService>();
            var bidDto = new BidDto { Account = "Account1", Type = "Type1", BidQuantity = 10 };
            var createdId = 1;
            mockService.Setup(service => service.CreateAsync(bidDto))
                       .ReturnsAsync(createdId);
            var sut = new BidController(mockService.Object);

            // Act
            var result = await sut.CreateBid(bidDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            Assert.IsNotNull((result as CreatedAtActionResult)?.Value);
        }

        [TestMethod]
        public async Task UpdateBid_ReturnNotFound_WhenBidDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<IBidService>();
            int bidId = 1;
            var bidDto = new BidDto { Account = "Account1", Type = "Type1", BidQuantity = 10 };
            mockService.Setup(service => service.UpdateAsync(bidId, bidDto))
                       .ReturnsAsync((BidDto?)null);
            var sut = new BidController(mockService.Object);

            // Act
            var result = await sut.UpdateBid(bidId, bidDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task UpdateBid_ReturnOk_WhenBidIsUpdated()
        {
            // Arrange
            var mockService = new Mock<IBidService>();
            int bidId = 1;
            var bidDto = new BidDto { Account = "Account1", Type = "Type1", BidQuantity = 10 };
            var updatedBidDto = new BidDto { Id = bidId, Account = "Account1Updated", Type = "Type1Updated", BidQuantity = 10 };
            mockService.Setup(service => service.UpdateAsync(bidId, bidDto))
                       .ReturnsAsync(updatedBidDto);
            var sut = new BidController(mockService.Object);

            // Act
            var result = await sut.UpdateBid(bidId, bidDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task DeleteBid_ReturnNotFound_WhenBidDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<IBidService>();
            int bidId = 1;
            mockService.Setup(service => service.DeleteAsync(bidId))
                       .ReturnsAsync(false);
            var sut = new BidController(mockService.Object);

            // Act
            var result = await sut.DeleteBid(bidId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task DeleteBid_ReturnNoContent_WhenBidIsDeleted()
        {
            // Arrange
            var mockService = new Mock<IBidService>();
            int bidId = 1;
            mockService.Setup(service => service.DeleteAsync(bidId))
                       .ReturnsAsync(true);
            var sut = new BidController(mockService.Object);

            // Act
            var result = await sut.DeleteBid(bidId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }
    }
}

