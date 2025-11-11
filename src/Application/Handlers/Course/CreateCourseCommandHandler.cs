using AutoMapper;
using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Infrastructure;
using MediatR;

namespace LearnHub.Back.Application.Handlers.Course
{
    public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, CourseDto>
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public CreateCourseCommandHandler(IMapper mapper, ApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<CourseDto> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            var course = _mapper.Map<Domain.Course>(request);
            _context.Courses.Add(course);

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CourseDto>(course);
        }
    }
}