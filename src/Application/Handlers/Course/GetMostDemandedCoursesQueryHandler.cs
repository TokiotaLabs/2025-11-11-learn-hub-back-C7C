using AutoMapper;
using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Back.Application.Handlers.Course
{
    public class GetMostDemandedCoursesQueryHandler : IRequestHandler<GetMostDemandedCoursesQuery, List<CourseDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetMostDemandedCoursesQueryHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CourseDto>> Handle(GetMostDemandedCoursesQuery request, CancellationToken cancellationToken)
        {
            var allCourses = await _context.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Enrollments)
                .ToListAsync(cancellationToken);

            var sortedCourses = allCourses
                .OrderByDescending(c => c.Enrollments?.Count ?? 0)
                .Take(request.Limit)
                .ToList();

            return _mapper.Map<List<CourseDto>>(sortedCourses);
        }
    }
}
