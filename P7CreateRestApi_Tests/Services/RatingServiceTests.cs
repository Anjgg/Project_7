using AutoMapper;
using Moq;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Dto;
using P7CreateRestApi.Repositories;
using P7CreateRestApi.Services;

namespace P7CreateRestApi_Tests.Services
{
    [TestClass]
    public class RatingServiceTests
    {
        [TestMethod]
        public async Task GetAllAsync_ReturnListOfRatingDto_WhenRatingsAreFound()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Rating>>();
            var mockMapper = new Mock<IMapper>();
            var ratings = new List<Rating>
            {
                new Rating { Id = 1, Fitch = "test1" },
                new Rating { Id = 2, Fitch = "test2" }
            };
            mockRepository.Setup(repo => repo.GetAllAsync())
                          .ReturnsAsync(ratings);
            mockMapper.Setup(m => m.Map<RatingDto>(It.IsAny<Rating>()))
                      .Returns((Rating rating) => new RatingDto
                      {
                          Id = rating.Id,
                          Fitch = rating.Fitch
                      });
            var ratingService = new RatingService(mockRepository.Object, mockMapper.Object);

            // Act
            var result = await ratingService.GetAllAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, result[0].Id);
            Assert.AreEqual(2, result[1].Id);
        }

        [TestMethod]
        public async Task GetAllAsync_ReturnEmptyList_WhenNoRatingsFound()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Rating>>();
            var mockMapper = new Mock<IMapper>();
            mockRepository.Setup(repo => repo.GetAllAsync())
                          .ReturnsAsync(new List<Rating>());
            var ratingService = new RatingService(mockRepository.Object, mockMapper.Object);

            // Act
            var result = await ratingService.GetAllAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task GetByIdAsync_ReturnNull_WhenRatingDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Rating>>();
            var mockMapper = new Mock<IMapper>();
            int ratingId = 1;
            mockRepository.Setup(repo => repo.GetByIdAsync(ratingId))
                          .ReturnsAsync((Rating?)null);
            var ratingService = new RatingService(mockRepository.Object, mockMapper.Object);

            // Act
            var result = await ratingService.GetByIdAsync(ratingId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetByIdAsync_ReturnRatingDto_WhenRatingExist()
        {
            var mockRepository = new Mock<IRepository<Rating>>();
            var mockMapper = new Mock<IMapper>();
            int ratingId = 1;
            var rating = new Rating { Id = ratingId, Fitch = "testFitch" };
            mockRepository.Setup(repo => repo.GetByIdAsync(ratingId))
                          .ReturnsAsync(rating);
            mockMapper.Setup(m => m.Map<RatingDto>(It.IsAny<Rating>()))
                        .Returns((Rating r) => new RatingDto
                        {
                            Id = r.Id,
                            Fitch = r.Fitch
                        });
            var ratingService = new RatingService(mockRepository.Object, mockMapper.Object);

            // Act
            var result = await ratingService.GetByIdAsync(ratingId);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task CreateAsync_ReturnNewRatingId_WhenCreationSucceeds()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Rating>>();
            var mockMapper = new Mock<IMapper>();
            var ratingDto = new RatingDto { Fitch = "newFitch" };
            var rating = new Rating { Id = 1, Fitch = "newFitch" };
            mockMapper.Setup(m => m.Map<Rating>(It.IsAny<RatingDto>()))
                      .Returns(rating);
            mockRepository.Setup(repo => repo.AddAsync(rating))
                          .ReturnsAsync(rating);
            var ratingService = new RatingService(mockRepository.Object, mockMapper.Object);

            // Act
            var result = await ratingService.CreateAsync(ratingDto);

            // Assert
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public async Task UpdateAsync_ReturnNull_WhenRatingDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Rating>>();
            var mockMapper = new Mock<IMapper>();
            int ratingId = 1;
            var ratingDto = new RatingDto { Fitch = "updatedFitch" };
            mockRepository.Setup(repo => repo.GetByIdAsync(ratingId))
                          .ReturnsAsync((Rating?)null);
            var ratingService = new RatingService(mockRepository.Object, mockMapper.Object);

            // Act
            var result = await ratingService.UpdateAsync(ratingId, ratingDto);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task UpdateAsync_ReturnUpdatedRatingDto_WhenRatingExists()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Rating>>();
            var mockMapper = new Mock<IMapper>();
            int ratingId = 1;
            var existingRating = new Rating { Id = ratingId, Fitch = "oldFitch" };
            var ratingDto = new RatingDto { Fitch = "updatedFitch" };
            var updatedRating = new Rating { Id = ratingId, Fitch = "updatedFitch" };
            mockRepository.Setup(repo => repo.GetByIdAsync(ratingId))
                          .ReturnsAsync(existingRating);
            mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Rating>()))
                          .ReturnsAsync(updatedRating);
            mockMapper.Setup(m => m.Map<RatingDto>(It.IsAny<Rating>()))
                      .Returns((Rating r) => new RatingDto
                      {
                          Id = r.Id,
                          Fitch = r.Fitch
                      });
            
            var ratingService = new RatingService(mockRepository.Object, mockMapper.Object);

            // Act
            var result = await ratingService.UpdateAsync(ratingId, ratingDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(ratingId, result!.Id);
            Assert.AreEqual("updatedFitch", result.Fitch);
        }

        [TestMethod]
        public async Task DeleteAsync_ReturnFalse_WhenRatingDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Rating>>();
            int ratingId = 1;
            mockRepository.Setup(repo => repo.GetByIdAsync(ratingId))
                          .ReturnsAsync((Rating?)null);
            var ratingService = new RatingService(mockRepository.Object, Mock.Of<IMapper>());

            // Act
            var result = await ratingService.DeleteAsync(ratingId);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task DeleteAsync_ReturnTrue_WhenRatingExists()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Rating>>();
            int ratingId = 1;
            var existingRating = new Rating { Id = ratingId, Fitch = "testFitch" };
            mockRepository.Setup(repo => repo.GetByIdAsync(ratingId))
                          .ReturnsAsync(existingRating);
            var ratingService = new RatingService(mockRepository.Object, Mock.Of<IMapper>());

            // Act
            var result = await ratingService.DeleteAsync(ratingId);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
