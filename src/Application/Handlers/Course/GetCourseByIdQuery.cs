using MediatR;
using LearnHub.Back.Application.DTOs;
using System;

namespace LearnHub.Back.Application.Handlers.Course
{
    public class GetCourseByIdQuery : IRequest<CourseDto?>
    {
        public Guid Id { get; set; }
    }
}