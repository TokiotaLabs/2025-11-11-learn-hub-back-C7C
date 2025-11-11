using MediatR;
using LearnHub.Back.Application.DTOs;

namespace LearnHub.Back.Application.Handlers.Enrollment
{
    public class CreateEnrollmentCommand : IRequest<EnrollmentDto>
    {
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
        public string SchedulePreference { get; set; }
        public string Status { get; set; } = "Pending"; // Default value
    }
}