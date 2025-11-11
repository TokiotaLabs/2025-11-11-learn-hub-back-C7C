using MediatR;

namespace LearnHub.Back.Application.Handlers.Enrollment
{
    public class UpdateEnrollmentCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public string SchedulePreference { get; set; }
        public Guid? PaymentId { get; set; }
    }
}