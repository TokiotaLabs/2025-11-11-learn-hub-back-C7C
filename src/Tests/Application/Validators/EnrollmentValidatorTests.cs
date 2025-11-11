using AutoFixture;
using FluentValidation.TestHelper;
using LearnHub.Back.Application.Handlers.Enrollment;
using LearnHub.Back.Tests.Configuration;

namespace LearnHub.Back.Tests.Application.Validators;

[TestFixture]
public class EnrollmentValidatorTests
{
    private CreateEnrollmentCommandValidator _createValidator;
    private UpdateEnrollmentCommandValidator _updateValidator;
    private IFixture _fixture;

    [SetUp]
    public void Setup()
    {
        _createValidator = new CreateEnrollmentCommandValidator();
        _updateValidator = new UpdateEnrollmentCommandValidator();
        _fixture = new Fixture().Customize(new TestCustomization());
        
        // Configure AutoFixture to generate valid enrollment data
        _fixture.Customize<CreateEnrollmentCommand>(c => c
            .With(x => x.StudentId, Guid.NewGuid())
            .With(x => x.CourseId, Guid.NewGuid())
            .With(x => x.SchedulePreference, "Morning classes")
            .With(x => x.Status, "Pending"));

        _fixture.Customize<UpdateEnrollmentCommand>(c => c
            .With(x => x.Id, Guid.NewGuid())
            .With(x => x.SchedulePreference, "Morning classes")
            .With(x => x.Status, "Pending"));
    }

    [Test]
    [AutoMoqData]
    public void CreateEnrollmentCommand_WithValidData_ShouldNotHaveValidationErrors(CreateEnrollmentCommand command)
    {
        // Arrange
        command.Status = "Pending";
        command.SchedulePreference = "Morning classes"; // Ensure valid length

        // Act & Assert
        var result = _createValidator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void CreateEnrollmentCommand_WithEmptyStudentId_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateEnrollmentCommand { StudentId = Guid.Empty };

        // Act & Assert
        var result = _createValidator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.StudentId);
    }

    [Test]
    public void CreateEnrollmentCommand_WithEmptyCourseId_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateEnrollmentCommand { CourseId = Guid.Empty };

        // Act & Assert
        var result = _createValidator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.CourseId);
    }

    [Test]
    public void CreateEnrollmentCommand_WithInvalidStatus_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateEnrollmentCommand { Status = "InvalidStatus" };

        // Act & Assert
        var result = _createValidator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Status);
    }

    [Test]
    public void CreateEnrollmentCommand_WithLongSchedulePreference_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateEnrollmentCommand 
        { 
            SchedulePreference = new string('A', 51) // 51 characters
        };

        // Act & Assert
        var result = _createValidator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.SchedulePreference);
    }

    [Test]
    [AutoMoqData]
    public void UpdateEnrollmentCommand_WithValidData_ShouldNotHaveValidationErrors(UpdateEnrollmentCommand command)
    {
        // Arrange
        command.Status = "Approved";
        command.SchedulePreference = "Morning classes"; // Ensure valid length

        // Act & Assert
        var result = _updateValidator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void UpdateEnrollmentCommand_WithEmptyId_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateEnrollmentCommand { Id = Guid.Empty };

        // Act & Assert
        var result = _updateValidator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void UpdateEnrollmentCommand_WithInvalidStatus_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateEnrollmentCommand { Status = "InvalidStatus" };

        // Act & Assert
        var result = _updateValidator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Status);
    }

    [Test]
    public void UpdateEnrollmentCommand_WithValidEnumStatus_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new UpdateEnrollmentCommand { Status = "Pending" };

        // Act & Assert
        var result = _updateValidator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Status);
    }
}