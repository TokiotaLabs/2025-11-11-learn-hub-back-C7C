using MediatR;
using LearnHub.Back.Application.DTOs;
using System.Collections.Generic;

namespace LearnHub.Back.Application.Handlers.Course
{
    public class GetMostDemandedCoursesQuery : IRequest<List<CourseDto>>
    {
        public int Limit { get; set; } = 10;
    }
}
