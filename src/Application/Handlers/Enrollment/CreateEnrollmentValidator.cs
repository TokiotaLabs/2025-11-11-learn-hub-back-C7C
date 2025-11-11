using FluentValidation;

namespace LearnHub.Back.Application.Handlers.Enrollment
{
    public class CreateEnrollmentValidator : AbstractValidator<CreateEnrollmentCommand>
    {
        public CreateEnrollmentValidator()
        {
            // Agregar más reglas de validación según sea necesario
        }
    }
}