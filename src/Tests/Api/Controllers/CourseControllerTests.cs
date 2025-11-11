using FluentAssertions;
using LearnHub.Back.Api.Controllers;
using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Application.Handlers.Course;
using LearnHub.Back.Tests.Configuration;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LearnHub.Back.Tests.Api.Controllers;

[TestFixture]
public class CourseControllerTests
{
    [Test]
    [AutoMoqData]
    public async Task GetAll_ShouldReturnOkWithCourses(
        List<CourseDto> courses,
        [Frozen] Mock<IMediator> mediatorMock,
        CourseController sut)
    {
        // Arrange
        mediatorMock.Setup(x => x.Send(It.IsAny<GetAllCoursesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(courses);

        // Act
        var result = await sut.GetAll();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(courses);
    }

    [Test]
    [AutoMoqData]
    public async Task GetById_WithExistingId_ShouldReturnOkWithCourse(
        CourseDto course,
        Guid courseId,
        [Frozen] Mock<IMediator> mediatorMock,
        CourseController sut)
    {
        // Arrange
        mediatorMock.Setup(x => x.Send(It.Is<GetCourseByIdQuery>(q => q.Id == courseId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(course);

        // Act
        var result = await sut.GetById(courseId);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(course);
    }

    [Test]
    [AutoMoqData]
    public async Task GetById_WithNonExistingId_ShouldReturnNotFound(
        Guid courseId,
        [Frozen] Mock<IMediator> mediatorMock,
        CourseController sut)
    {
        // Arrange
        mediatorMock.Setup(x => x.Send(It.Is<GetCourseByIdQuery>(q => q.Id == courseId), It.IsAny<CancellationToken>()))
            .ReturnsAsync((CourseDto)null);

        // Act
        var result = await sut.GetById(courseId);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    [AutoMoqData]
    public async Task Create_WithValidCommand_ShouldReturnCreatedAtAction(
        CreateCourseCommand command,
        CourseDto createdCourse,
        [Frozen] Mock<IMediator> mediatorMock,
        CourseController sut)
    {
        // Arrange
        mediatorMock.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdCourse);

        // Act
        var result = await sut.Create(command);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result as CreatedAtActionResult;
        createdResult!.Value.Should().BeEquivalentTo(createdCourse);
        createdResult.ActionName.Should().Be(nameof(CourseController.GetById));
    }

    [Test]
    [AutoMoqData]
    public async Task Update_WithMatchingIds_ShouldReturnNoContent(
        Guid courseId,
        UpdateCourseCommand command,
        [Frozen] Mock<IMediator> mediatorMock,
        CourseController sut)
    {
        // Arrange
        command.Id = courseId;
        mediatorMock.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        // Act
        var result = await sut.Update(courseId, command);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Test]
    [AutoMoqData]
    public async Task Update_WithMismatchingIds_ShouldReturnBadRequest(
        Guid courseId,
        UpdateCourseCommand command,
        CourseController sut)
    {
        // Arrange
        command.Id = Guid.NewGuid(); // Different ID than the route parameter

        // Act
        var result = await sut.Update(courseId, command);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Test]
    [AutoMoqData]
    public async Task Delete_WithExistingId_ShouldReturnNoContent(
        Guid courseId,
        [Frozen] Mock<IMediator> mediatorMock,
        CourseController sut)
    {
        // Arrange
        mediatorMock.Setup(x => x.Send(It.Is<DeleteCourseCommand>(c => c.Id == courseId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        // Act
        var result = await sut.Delete(courseId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }
}