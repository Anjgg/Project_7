using Microsoft.AspNetCore.Mvc;
using Moq;
using P7CreateRestApi.Controllers;
using P7CreateRestApi.Dto;
using P7CreateRestApi.Services;

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
        public async Task GetCurvePoint_ReturnOk_WhenCurvePointExists()
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
        public async Task GetCurvePoint_ReturnNotFound_WhenCurvePointDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<ICurvePointService>();
            int curvePointId = 1;
            mockService.Setup(service => service.GetByIdAsync(curvePointId))
                       .ReturnsAsync((CurvePointDto?)null);
            var sut = new CurvePointController(mockService.Object);

            // Act
            var result = await sut.GetCurvePoint(curvePointId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task CreateCurvePoint_ReturnCreatedAtActionResult_WhenCurvePointIsCreated()
        {
            // Arrange
            var mockService = new Mock<ICurvePointService>();
            var curvePointDto = new CurvePointDto { CurvePointValue = 12, AsOfDate = DateTime.Now, CreationDate = DateTime.Now, Term = 34 };
            var createdId = 1;
            mockService.Setup(service => service.CreateAsync(curvePointDto))
                       .ReturnsAsync(createdId);
            var sut = new CurvePointController(mockService.Object);

            // Act
            var result = await sut.CreateCurvePoint(curvePointDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            Assert.IsNotNull((result as CreatedAtActionResult)?.Value);
        }

        [TestMethod]
        public async Task UpdateCurvePoint_ReturnNotFound_WhenCurvePointDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<ICurvePointService>();
            int curvePointId = 1;
            var curvePointDto = new CurvePointDto { Id = 1, CurvePointValue = 12, AsOfDate = DateTime.Now, CreationDate = DateTime.Now, Term = 34 };
            mockService.Setup(service => service.UpdateAsync(curvePointId, curvePointDto))
                       .ReturnsAsync((CurvePointDto?)null);
            var sut = new CurvePointController(mockService.Object);

            // Act
            var result = await sut.UpdateCurvePoint(curvePointId, curvePointDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task UpdateCurvePoint_ReturnOk_WhenCurvePointIsUpdated()
        {
            // Arrange
            var mockService = new Mock<ICurvePointService>();
            int curvePointId = 1;
            var curvePointDto = new CurvePointDto { Id = 1, CurvePointValue = 12, AsOfDate = DateTime.Now, CreationDate = DateTime.Now, Term = 34 };
            var updatedCurvePointDto = new CurvePointDto { Id = 1, CurvePointValue = 45, AsOfDate = DateTime.Now, CreationDate = DateTime.Now, Term = 78 };
            mockService.Setup(service => service.UpdateAsync(curvePointId, curvePointDto))
                       .ReturnsAsync(updatedCurvePointDto);
            var sut = new CurvePointController(mockService.Object);

            // Act
            var result = await sut.UpdateCurvePoint(curvePointId, curvePointDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task DeleteCurvePoint_ReturnNotFound_WhenCurvePointDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<ICurvePointService>();
            int curvePointId = 1;
            mockService.Setup(service => service.DeleteAsync(curvePointId))
                       .ReturnsAsync(false);
            var sut = new CurvePointController(mockService.Object);

            // Act
            var result = await sut.DeleteCurvePoint(curvePointId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task DeleteCurvePoint_ReturnNoContent_WhenCurvePointIsDeleted()
        {
            // Arrange
            var mockService = new Mock<ICurvePointService>();
            int curvePointId = 1;
            mockService.Setup(service => service.DeleteAsync(curvePointId))
                       .ReturnsAsync(true);
            var sut = new CurvePointController(mockService.Object);

            // Act
            var result = await sut.DeleteCurvePoint(curvePointId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }
    }
}




