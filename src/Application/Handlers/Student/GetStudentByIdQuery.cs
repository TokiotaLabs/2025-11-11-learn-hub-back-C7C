using MediatR;
using LearnHub.Back.Application.DTOs;

namespace LearnHub.Back.Application.Handlers.Student
{
    public class GetStudentByIdQuery : IRequest<StudentDto?>
    {
        public Guid Id { get; set; }
    }
}