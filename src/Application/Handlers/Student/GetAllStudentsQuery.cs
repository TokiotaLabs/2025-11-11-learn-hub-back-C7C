using MediatR;
using LearnHub.Back.Application.DTOs;

namespace LearnHub.Back.Application.Handlers.Student
{
    public class GetAllStudentsQuery : IRequest<List<StudentDto>>
    {
    }
}