using MediatR;
using LearnHub.Back.Application.DTOs;

namespace LearnHub.Back.Application.Handlers.Enrollment
{
    public class GetEnrollmentByIdQuery : IRequest<EnrollmentDto?>
    {
        public Guid Id { get; set; }
    }
}