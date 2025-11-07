using AutoMapper;
using Moq;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Dto;
using P7CreateRestApi.Repositories;
using P7CreateRestApi.Services;

namespace P7CreateRestApi_Tests.Services
{
    [TestClass]
    public class CurvePointServiceTests
    {
        [TestMethod]
        public async Task GetAllAsync_ReturnListCurvePointDto_WhenCurvePointIsFound()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<CurvePoint>>();
            var mockMapper = new Mock<IMapper>();
            var curvePoints = new List<CurvePoint>
            {
                new CurvePoint { Id = 1, Term = 10.0 },
                new CurvePoint { Id = 2, Term = 15.0 }
            };
            mockRepository.Setup(repo => repo.GetAllAsync())
                          .ReturnsAsync(curvePoints);
            mockMapper.Setup(m => m.Map<CurvePointDto>(It.IsAny<CurvePoint>()))
                      .Returns((CurvePoint cp) => new CurvePointDto { Id = cp.Id, Term = cp.Term });
            var curvePointService = new CurvePointService(mockRepository.Object, mockMapper.Object);
            // Act
            var result = await curvePointService.GetAllAsync();
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, result[0].Id);
            Assert.AreEqual(2, result[1].Id);
        }

        [TestMethod]
        public async Task GetAllAsync_ReturnEmptyList_WhenNoCurvePointFound()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<CurvePoint>>();
            var mockMapper = new Mock<IMapper>();
            mockRepository.Setup(repo => repo.GetAllAsync())
                          .ReturnsAsync(new List<CurvePoint>());
            var curvePointService = new CurvePointService(mockRepository.Object, mockMapper.Object);

            // Act
            var result = await curvePointService.GetAllAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task GetByIdAsync_ReturnNull_WhenCurvePointDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<CurvePoint>>();
            var mockMapper = new Mock<IMapper>();
            int curvePointId = 1;
            mockRepository.Setup(repo => repo.GetByIdAsync(curvePointId))
                          .ReturnsAsync((CurvePoint?)null);
            var curvePointService = new CurvePointService(mockRepository.Object, mockMapper.Object);

            // Act
            var result = await curvePointService.GetByIdAsync(curvePointId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetByIdAsync_ReturnCurvePointDto_WhenCurvePointExists()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<CurvePoint>>();
            var mockMapper = new Mock<IMapper>();
            int curvePointId = 1;
            var curvePoint = new CurvePoint { Id = curvePointId, Term = 10.0 };
            var curvePointDto = new CurvePointDto { Id = curvePointId, Term = 10.0 };
            mockRepository.Setup(repo => repo.GetByIdAsync(curvePointId))
                          .ReturnsAsync(curvePoint);
            mockMapper.Setup(m => m.Map<CurvePointDto>(curvePoint))
                      .Returns(curvePointDto);
            var curvePointService = new CurvePointService(mockRepository.Object, mockMapper.Object);

            // Act
            var result = await curvePointService.GetByIdAsync(curvePointId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(curvePointDto.Id, result!.Id);
            Assert.AreEqual(curvePointDto.Term, result.Term);
        }

        [TestMethod]
        public async Task CreateAsync_ReturnsNewCurvePointId_WhenCurvePointIsCreated()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<CurvePoint>>();
            var mockMapper = new Mock<IMapper>();
            var curvePointDto = new CurvePointDto { Term = 25.0 };
            var curvePoint = new CurvePoint { Term = 25.0 };
            var createdCurvePoint = new CurvePoint { Id = 1, Term = 25.0 };
            mockMapper.Setup(m => m.Map<CurvePoint>(curvePointDto))
                      .Returns(curvePoint);
            mockRepository.Setup(repo => repo.AddAsync(curvePoint))
                          .ReturnsAsync(createdCurvePoint);
            var curvePointService = new CurvePointService(mockRepository.Object, mockMapper.Object);

            // Act
            var resultId = await curvePointService.CreateAsync(curvePointDto);

            // Assert
            Assert.AreEqual(1, resultId);
        }

        [TestMethod]
        public async Task UpdateAsync_ReturnNull_WhenCurvePointDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<CurvePoint>>();
            var mockMapper = new Mock<IMapper>();
            int curvePointId = 1;
            var curvePointDto = new CurvePointDto { Term = 20.0 };
            mockRepository.Setup(repo => repo.GetByIdAsync(curvePointId))
                          .ReturnsAsync((CurvePoint?)null);
            var curvePointService = new CurvePointService(mockRepository.Object, mockMapper.Object);

            // Act
            var result = await curvePointService.UpdateAsync(curvePointId, curvePointDto);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task UpdateAsync_ReturnUpdatedCurvePointDto_WhenCurvePointExists()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<CurvePoint>>();
            var mockMapper = new Mock<IMapper>();
            int curvePointId = 1;
            var existingCurvePoint = new CurvePoint { Id = curvePointId, Term = 20 };
            var curvePointDto = new CurvePointDto { Term = 30 };
            var updatedCurvePoint = new CurvePoint { Id = curvePointId, Term = 30 };
            mockRepository.Setup(repo => repo.GetByIdAsync(curvePointId))
                          .ReturnsAsync(existingCurvePoint);
            mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<CurvePoint>()))
                          .ReturnsAsync(updatedCurvePoint);
            mockMapper.Setup(m => m.Map<CurvePointDto>(It.IsAny<CurvePoint>()))
                      .Returns((CurvePoint cp) => new CurvePointDto { Id = cp.Id, Term = cp.Term });
            var curvePointService = new CurvePointService(mockRepository.Object, mockMapper.Object);

            // Act
            var result = await curvePointService.UpdateAsync(curvePointId, curvePointDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(curvePointId, result?.Id);
            Assert.AreEqual(30, result?.Term);
        }

        [TestMethod]
        public async Task DeleteAsync_ReturnsTrue_WhenCurvePointIsDeleted()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<CurvePoint>>();
            int curvePointId = 1;
            var existingBid = new CurvePoint { Id = curvePointId, Term = 20 };
            mockRepository.Setup(repo => repo.GetByIdAsync(curvePointId))
                          .ReturnsAsync(existingBid);
            var curvePointService = new CurvePointService(mockRepository.Object, Mock.Of<IMapper>());

            // Act
            var result = await curvePointService.DeleteAsync(curvePointId);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task DeleteAsync_ReturnsFalse_WhenCurvePointDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<CurvePoint>>();
            int curvePointId = 1;
            mockRepository.Setup(repo => repo.GetByIdAsync(curvePointId))
                          .ReturnsAsync((CurvePoint?)null);
            var curvePointService = new CurvePointService(mockRepository.Object, Mock.Of<IMapper>());
            // Act
            var result = await curvePointService.DeleteAsync(curvePointId);
            // Assert
            Assert.IsFalse(result);
        }


    }
}
