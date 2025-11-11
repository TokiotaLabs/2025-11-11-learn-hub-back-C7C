using FluentValidation;

namespace LearnHub.Back.Application.Handlers.Enrollment
{
    public class CreateEnrollmentCommandValidator : AbstractValidator<CreateEnrollmentCommand>
    {
        public CreateEnrollmentCommandValidator()
        {
            RuleFor(x => x.StudentId)
                .NotEmpty().WithMessage("Student ID is required");

            RuleFor(x => x.CourseId)
                .NotEmpty().WithMessage("Course ID is required");

            RuleFor(x => x.SchedulePreference)
                .NotEmpty().WithMessage("Schedule preference is required")
                .MaximumLength(50).WithMessage("Schedule preference cannot exceed 50 characters");

            RuleFor(x => x.Status)
                .Must(x => x == "Pending" || x == "Approved" || x == "Rejected")
                .WithMessage("Status must be 'Pending', 'Approved' or 'Rejected'");
        }
    }
}