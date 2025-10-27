using Microsoft.AspNetCore.Mvc;
using Moq;
using P7CreateRestApi.Dto;
using P7CreateRestApi.Services;
using P7CreateRestApi.Controllers;

namespace P7CreateRestApi_Tests.Controllers
{
    [TestClass]
    public class CurvePointControllerTests
    {
        [TestMethod]
        public async Task ListAllCurvePoints_ReturnOk_WhenListExist()
        {
            // Arrange
            var mockService = new Mock<ICurvePointService>();

            mockService.Setup(service => service.GetAllAsync())
                       .ReturnsAsync(new List<CurvePointDto>
                       {
                           new CurvePointDto { Id = 1, CurvePointValue = 12, AsOfDate = DateTime.Now, CreationDate = DateTime.Now, Term = 34 },
                           new CurvePointDto { Id = 2, CurvePointValue = 56, AsOfDate = DateTime.Now, CreationDate = DateTime.Now, Term = 78 },
                       });
            var sut = new CurvePointController(mockService.Object);

            // Act
            var result = await sut.ListAllCurvePoint();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsNotNull((result as OkObjectResult)?.Value);
        }

        [TestMethod]
        public async Task ListAllCurvePoint_ReturnNotFound_WhenListDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<ICurvePointService>();
            mockService.Setup(service => service.GetAllAsync())!
                       .ReturnsAsync((List<CurvePointDto>?)null);
            var sut = new CurvePointController(mockService.Object);

            // Act
            var result = await sut.ListAllCurvePoint();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetCurvePoint_ReturnOk_WhenBidExists()
        {
            // Arrange
            var mockService = new Mock<ICurvePointService>();
            int curvePointId = 1;
            mockService.Setup(service => service.GetByIdAsync(curvePointId))
                       .ReturnsAsync(new CurvePointDto { Id = curvePointId, CurvePointValue = 56, AsOfDate = DateTime.Now, CreationDate = DateTime.Now, Term = 78 });
            var sut = new CurvePointController(mockService.Object);

            // Act
            var result = await sut.GetCurvePoint(curvePointId);

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
            var createdBidDto = new BidDto { Id = 1, Account = "Account1", Type = "Type1", BidQuantity = 10 };
            mockService.Setup(service => service.CreateAsync(bidDto))
                       .ReturnsAsync(createdBidDto);
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

        [TestMethod]
        public async Task CreateBid_ReturnBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            var mockService = new Mock<IBidService>();
            var bidDto = new BidDto { Account = "", Type = "Type1", BidQuantity = 10 }; // Invalid Account
            var sut = new BidController(mockService.Object);
            sut.ModelState.AddModelError("Account", "Account is required.");

            // Act
            var result = await sut.CreateBid(bidDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task UpdateBid_ReturnBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            var mockService = new Mock<IBidService>();
            int bidId = 1;
            var bidDto = new BidDto { Account = "", Type = "Type1", BidQuantity = 10 }; // Invalid Account
            var sut = new BidController(mockService.Object);
            sut.ModelState.AddModelError("Account", "Account is required.");
            // Act
            var result = await sut.UpdateBid(bidId, bidDto);
            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task CreateBid_ReturnObjectResult_WhenProblemInCreation()
        {
            // Arrange
            var mockService = new Mock<IBidService>();
            var bidDto = new BidDto { Account = "Account1", Type = "Type1", BidQuantity = 10 };
            mockService.Setup(service => service.CreateAsync(bidDto))
                       .ReturnsAsync((BidDto?)null);
            var sut = new BidController(mockService.Object);
            // Act
            var result = await sut.CreateBid(bidDto);
            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            var objectResult = result as ObjectResult;
            Assert.AreEqual(500, objectResult?.StatusCode);
        }
    }
}




