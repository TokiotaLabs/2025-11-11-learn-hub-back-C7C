using MediatR;

namespace LearnHub.Back.Application.Handlers.Course
{
    public class DeleteCourseCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}