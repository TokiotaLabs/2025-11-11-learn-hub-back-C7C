using FluentAssertions;
using LearnHub.Back.Api.Controllers;
using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Application.Handlers.Enrollment;
using LearnHub.Back.Tests.Configuration;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using AutoFixture;

namespace LearnHub.Back.Tests.Api.Controllers;

[TestFixture]
public class EnrollmentControllerTests
{
    private IFixture _fixture;
    private Mock<IMediator> _mediatorMock;
    private EnrollmentController _controller;

    [SetUp]
    public void Setup()
    {
        _fixture = AutoFixtureConfiguration.Create();
        _mediatorMock = new Mock<IMediator>();
        _controller = new EnrollmentController(_mediatorMock.Object);
    }

    [Test]
    public async Task GetAll_ShouldReturnOkWithEnrollments()
    {
        // Arrange
        var enrollments = _fixture.CreateMany<EnrollmentDto>(3).ToList();
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetAllEnrollmentsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(enrollments);

        // Act
        var result = await _controller.GetAll();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(enrollments);
    }

    [Test]
    public async Task GetById_WithExistingId_ShouldReturnOkWithEnrollment()
    {
        // Arrange
        var id = Guid.NewGuid();
        var enrollment = _fixture.Create<EnrollmentDto>();
        _mediatorMock.Setup(x => x.Send(It.Is<GetEnrollmentByIdQuery>(q => q.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(enrollment);

        // Act
        var result = await _controller.GetById(id);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(enrollment);
    }

    [Test]
    public async Task Create_WithValidCommand_ShouldReturnCreatedAtAction()
    {
        // Arrange
        var command = _fixture.Create<CreateEnrollmentCommand>();
        var createdEnrollment = _fixture.Create<EnrollmentDto>();
        _mediatorMock.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdEnrollment);

        // Act
        var result = await _controller.Create(command);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result as CreatedAtActionResult;
        createdResult!.Value.Should().BeEquivalentTo(createdEnrollment);
        createdResult.ActionName.Should().Be(nameof(EnrollmentController.GetById));
    }

    [Test]
    public async Task Update_WithMatchingIds_ShouldReturnNoContent()
    {
        // Arrange
        var id = Guid.NewGuid();
        var command = _fixture.Build<UpdateEnrollmentCommand>()
            .With(x => x.Id, id)
            .Create();

        _mediatorMock.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        // Act
        var result = await _controller.Update(id, command);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Test]
    public async Task Update_WithMismatchingIds_ShouldReturnBadRequest()
    {
        // Arrange
        var id = Guid.NewGuid();
        var command = _fixture.Build<UpdateEnrollmentCommand>()
            .With(x => x.Id, Guid.NewGuid()) // Different ID
            .Create();

        // Act
        var result = await _controller.Update(id, command);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Test]
    public async Task Delete_WithExistingId_ShouldReturnNoContent()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mediatorMock.Setup(x => x.Send(It.Is<DeleteEnrollmentCommand>(c => c.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        // Act
        var result = await _controller.Delete(id);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }
}