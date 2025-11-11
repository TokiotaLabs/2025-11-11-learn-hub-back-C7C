using MediatR;

namespace LearnHub.Back.Application.Handlers.Enrollment
{
    public class DeleteEnrollmentCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}