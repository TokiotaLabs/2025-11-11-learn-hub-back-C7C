using FluentValidation.TestHelper;
using LearnHub.Back.Application.Handlers.Student;
using LearnHub.Back.Tests.Configuration;
using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using LearnHub.Back.Application;

namespace LearnHub.Back.Tests.Application.Validators;

[TestFixture]
public class StudentValidatorTests
{
    private CreateStudentCommandValidator _createValidator;
    private UpdateStudentCommandValidator _updateValidator;
    private IFixture _fixture;

    [SetUp]
    public void Setup()
    {
        _createValidator = new CreateStudentCommandValidator();
        _updateValidator = new UpdateStudentCommandValidator();
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Test]
    public void CreateStudentCommand_WithValidData_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new CreateStudentCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@test.com"
        };

        // Act & Assert
        var result = _createValidator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void CreateStudentCommand_WithShortFirstName_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateStudentCommand { FirstName = "A" }; // Too short

        // Act & Assert
        var result = _createValidator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Test]
    public void CreateStudentCommand_WithInvalidEmail_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateStudentCommand { Email = "invalid-email" };

        // Act & Assert
        var result = _createValidator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Test]
    public void CreateStudentCommand_WithEmptyLastName_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateStudentCommand { LastName = string.Empty };

        // Act & Assert
        var result = _createValidator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }

    [Test]
    public void UpdateStudentCommand_WithValidData_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new UpdateStudentCommand
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com"
        };

        // Act & Assert
        var result = _updateValidator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void UpdateStudentCommand_WithEmptyId_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateStudentCommand { Id = Guid.Empty };

        // Act & Assert
        var result = _updateValidator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void UpdateStudentCommand_WithInvalidEmailFormat_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateStudentCommand { Email = "not-an-email" };

        // Act & Assert
        var result = _updateValidator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Test]
    public void UpdateStudentCommand_WithLongFirstName_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateStudentCommand { FirstName = new string('A', 51) }; // 51 characters

        // Act & Assert
        var result = _updateValidator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }
}