using LearnHub.Back.Application.DTOs;
using MediatR;

namespace LearnHub.Back.Application.Handlers.Course
{
    public class CreateCourseCommand : IRequest<CourseDto>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
        public decimal Price { get; set; }
        public string Prerequisites { get; set; }
        public Guid InstructorId { get; set; }
        public string Modality { get; set; }
        public string IncludedMaterials { get; set; }
        public string Certification { get; set; }
        public int AvailableSeats { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
    }
}