using Microsoft.AspNetCore.Mvc;
using Moq;
using P7CreateRestApi.Dto;
using P7CreateRestApi.Services;
using P7CreateRestApi.Controllers;

namespace P7CreateRestApi_Tests.Controllers
{
    [TestClass]
    public class RatingControllerTests
    {

        [TestMethod]
        public async Task ListAllRatings_ReturnOk_WhenListExist()
        {
            // Arrange
            var mockService = new Mock<IRatingService>();

            mockService.Setup(service => service.GetAllAsync())
                       .ReturnsAsync(new List<RatingDto>
                       {
                           new RatingDto { Id = 1, Fitch = "test", Moodys = "test", OrderNumber = (byte)0, SandP = "test" },
                           new RatingDto { Id = 2, Fitch = "test", Moodys = "test", OrderNumber = (byte)2, SandP = "test" }
                       });
            var sut = new RatingController(mockService.Object);

            // Act
            var result = await sut.ListAllRating();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsNotNull((result as OkObjectResult)?.Value);
        }

        [TestMethod]
        public async Task ListAllRatings_ReturnNotFound_WhenListDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<IRatingService>();
            mockService.Setup(service => service.GetAllAsync())!
                       .ReturnsAsync((List<RatingDto>?)null);
            var sut = new RatingController(mockService.Object);

            // Act
            var result = await sut.ListAllRating();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetRating_ReturnOk_WhenRatingExists()
        {
            // Arrange
            var mockService = new Mock<IRatingService>();
            int ratingId = 1;
            mockService.Setup(service => service.GetByIdAsync(ratingId))
                       .ReturnsAsync(new RatingDto { Id = ratingId, Fitch = "test", Moodys = "test", OrderNumber = (byte)0, SandP = "test" });
            var sut = new RatingController(mockService.Object);

            // Act
            var result = await sut.GetRating(ratingId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsNotNull((result as OkObjectResult)?.Value);
        }

        [TestMethod]
        public async Task GetRating_ReturnNotFound_WhenRatingDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<IRatingService>();
            int ratingId = 1;
            mockService.Setup(service => service.GetByIdAsync(ratingId))
                       .ReturnsAsync((RatingDto?)null);
            var sut = new RatingController(mockService.Object);

            // Act
            var result = await sut.GetRating(ratingId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task CreateRating_ReturnCreatedAtActionResult_WhenRatingIsCreated()
        {
            // Arrange
            var mockService = new Mock<IRatingService>();
            var ratingDto = new RatingDto { Fitch = "test", Moodys = "test", OrderNumber = (byte)0, SandP = "test" };
            var createdId = 1;
            mockService.Setup(service => service.CreateAsync(ratingDto))
                       .ReturnsAsync(createdId);
            var sut = new RatingController(mockService.Object);

            // Act
            var result = await sut.CreateRating(ratingDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            Assert.IsNotNull((result as CreatedAtActionResult)?.Value);
        }

        [TestMethod]
        public async Task UpdateRating_ReturnNotFound_WhenRatingDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<IRatingService>();
            int ratingId = 1;
            var ratingDto = new RatingDto { Fitch = "test", Moodys = "test", OrderNumber = (byte)0, SandP = "test" };
            mockService.Setup(service => service.UpdateAsync(ratingId, ratingDto))
                       .ReturnsAsync((RatingDto?)null);
            var sut = new RatingController(mockService.Object);

            // Act
            var result = await sut.UpdateRating(ratingId, ratingDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task UpdateRating_ReturnOk_WhenRatingIsUpdated()
        {
            // Arrange
            var mockService = new Mock<IRatingService>();
            int ratingId = 1;
            var ratingDto = new RatingDto { Fitch = "test", Moodys = "test", OrderNumber = (byte)0, SandP = "test" };
            var updatedRatingDto = new RatingDto { Id = ratingId, Fitch = "test", Moodys = "test", OrderNumber = (byte)0, SandP = "test" };
            mockService.Setup(service => service.UpdateAsync(ratingId, ratingDto))
                       .ReturnsAsync(updatedRatingDto);
            var sut = new RatingController(mockService.Object);

            // Act
            var result = await sut.UpdateRating(ratingId, ratingDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task DeleteRating_ReturnNotFound_WhenRatingDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<IRatingService>();
            int ratingId = 1;
            mockService.Setup(service => service.DeleteAsync(ratingId))
                       .ReturnsAsync(false);
            var sut = new RatingController(mockService.Object);

            // Act
            var result = await sut.DeleteRating(ratingId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task DeleteRating_ReturnNoContent_WhenRatingIsDeleted()
        {
            // Arrange
            var mockService = new Mock<IRatingService>();
            int ratingId = 1;
            mockService.Setup(service => service.DeleteAsync(ratingId))
                       .ReturnsAsync(true);
            var sut = new RatingController(mockService.Object);

            // Act
            var result = await sut.DeleteRating(ratingId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }
    }
}


