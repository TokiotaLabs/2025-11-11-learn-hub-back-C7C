using MediatR;

namespace LearnHub.Back.Application.Handlers.Student
{
    public class DeleteStudentCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}