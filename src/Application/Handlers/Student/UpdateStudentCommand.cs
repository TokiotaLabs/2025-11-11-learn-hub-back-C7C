using MediatR;

namespace LearnHub.Back.Application.Handlers.Student
{
    public class UpdateStudentCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}