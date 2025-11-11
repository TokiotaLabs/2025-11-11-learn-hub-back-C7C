using AutoMapper;
using LearnHub.Back.Infrastructure;
using MediatR;

namespace LearnHub.Back.Application.Handlers.Course
{
    public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public UpdateCourseCommandHandler(IMapper mapper, ApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Unit> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
        {
            var course = await _context.Courses.FindAsync(request.Id, cancellationToken);
            _mapper.Map(request, course);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}