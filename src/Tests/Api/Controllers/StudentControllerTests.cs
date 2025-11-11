using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using LearnHub.Back.Api.Controllers;
using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Application.Handlers.Student;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LearnHub.Back.Tests.Api.Controllers;

[TestFixture]
public class StudentControllerTests
{
    private IFixture _fixture;
    private Mock<IMediator> _mediatorMock;

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        
        _mediatorMock = new Mock<IMediator>();
    }

    [Test]
    public async Task GetAll_ShouldReturnOkWithStudents()
    {
        // Arrange
        var students = _fixture.CreateMany<StudentDto>(3).ToList();
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetAllStudentsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(students);
        var sut = new StudentController(_mediatorMock.Object);

        // Act
        var result = await sut.GetAll();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(students);
    }

    [Test]
    public async Task GetById_WithExistingId_ShouldReturnOkWithStudent()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        var student = _fixture.Create<StudentDto>();
        _mediatorMock.Setup(x => x.Send(It.Is<GetStudentByIdQuery>(q => q.Id == studentId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);
        var sut = new StudentController(_mediatorMock.Object);

        // Act
        var result = await sut.GetById(studentId);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(student);
    }

    [Test]
    public async Task GetById_WithNonExistingId_ShouldReturnNotFound()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        _mediatorMock.Setup(x => x.Send(It.Is<GetStudentByIdQuery>(q => q.Id == studentId), It.IsAny<CancellationToken>()))
            .ReturnsAsync((StudentDto)null);
        var sut = new StudentController(_mediatorMock.Object);

        // Act
        var result = await sut.GetById(studentId);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task Create_WithValidCommand_ShouldReturnCreatedAtAction()
    {
        // Arrange
        var command = _fixture.Create<CreateStudentCommand>();
        var createdStudent = _fixture.Create<StudentDto>();
        _mediatorMock.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdStudent);
        var sut = new StudentController(_mediatorMock.Object);

        // Act
        var result = await sut.Create(command);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result as CreatedAtActionResult;
        createdResult!.Value.Should().BeEquivalentTo(createdStudent);
        createdResult.ActionName.Should().Be(nameof(StudentController.GetById));
    }

    [Test]
    public async Task Update_WithMatchingIds_ShouldReturnNoContent()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        var command = _fixture.Create<UpdateStudentCommand>();
        command.Id = studentId;
        _mediatorMock.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);
        var sut = new StudentController(_mediatorMock.Object);

        // Act
        var result = await sut.Update(studentId, command);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Test]
    public async Task Update_WithMismatchingIds_ShouldReturnBadRequest()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        var command = _fixture.Create<UpdateStudentCommand>();
        command.Id = Guid.NewGuid(); // Different ID than the route parameter
        var sut = new StudentController(_mediatorMock.Object);

        // Act
        var result = await sut.Update(studentId, command);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task Delete_WithExistingId_ShouldReturnNoContent()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        _mediatorMock.Setup(x => x.Send(It.Is<DeleteStudentCommand>(c => c.Id == studentId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);
        var sut = new StudentController(_mediatorMock.Object);

        // Act
        var result = await sut.Delete(studentId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }
}