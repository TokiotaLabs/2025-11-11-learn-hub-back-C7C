using FluentValidation;

namespace LearnHub.Back.Application.Handlers.Enrollment
{
    public class UpdateEnrollmentCommandValidator : AbstractValidator<UpdateEnrollmentCommand>
    {
        public UpdateEnrollmentCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("ID is required");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required")
                .Must(x => x == "Pending" || x == "Approved" || x == "Rejected")
                .WithMessage("Status must be 'Pending', 'Approved' or 'Rejected'");

            RuleFor(x => x.SchedulePreference)
                .NotEmpty().WithMessage("Schedule preference is required")
                .MaximumLength(50).WithMessage("Schedule preference cannot exceed 50 characters");

            // PaymentId is optional, so we don't need a NotEmpty rule
            RuleFor(x => x.PaymentId)
                .Must(x => !x.HasValue || x != Guid.Empty)
                .WithMessage("Payment ID cannot be an empty GUID when provided");
        }
    }
}