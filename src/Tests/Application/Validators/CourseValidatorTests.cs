using FluentValidation.TestHelper;
using LearnHub.Back.Application.Handlers.Course;
using LearnHub.Back.Tests.Configuration;
using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;

namespace LearnHub.Back.Tests.Application.Validators;

[TestFixture]
public class CourseValidatorTests
{
    private CreateCourseCommandValidator _createValidator;
    private UpdateCourseCommandValidator _updateValidator;
    private IFixture _fixture;

    [SetUp]
    public void Setup()
    {
        _createValidator = new CreateCourseCommandValidator();
        _updateValidator = new UpdateCourseCommandValidator();
        _fixture = new Fixture().Customize(new TestCustomization());
        
        var startDate = DateTime.Now.AddDays(1);
        var endDate = startDate.AddMonths(3);
        
        _fixture.Customize<CreateCourseCommand>(c => c
            .With(x => x.Title, "Test Course")
            .With(x => x.Description, "Test Description")
            .With(x => x.StartDate, startDate)
            .With(x => x.EndDate, endDate));

        _fixture.Customize<UpdateCourseCommand>(c => c
            .With(x => x.Id, Guid.NewGuid())
            .With(x => x.Title, "Test Course")
            .With(x => x.Description, "Test Description")
            .With(x => x.StartDate, startDate)
            .With(x => x.EndDate, endDate));
    }

    [Test]
    public void CreateCourseCommand_WithValidData_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = _fixture.Create<CreateCourseCommand>();

        // Act & Assert
        var result = _createValidator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void CreateCourseCommand_WithEmptyTitle_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateCourseCommand { Title = string.Empty };

        // Act & Assert
        var result = _createValidator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Test]
    public void CreateCourseCommand_WithStartDateAfterEndDate_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateCourseCommand 
        { 
            StartDate = DateTime.Now.AddDays(2),
            EndDate = DateTime.Now.AddDays(1)
        };

        // Act & Assert
        var result = _createValidator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.StartDate);
    }

    [Test]
    [AutoMoqData]
    public void UpdateCourseCommand_WithValidData_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = _fixture.Create<UpdateCourseCommand>();

        // Act & Assert
        var result = _updateValidator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void UpdateCourseCommand_WithEmptyId_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateCourseCommand { Id = Guid.Empty };

        // Act & Assert
        var result = _updateValidator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void UpdateCourseCommand_WithEmptyTitle_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateCourseCommand { Title = string.Empty };

        // Act & Assert
        var result = _updateValidator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }
}