using LearnHub.Back.Domain;
using System.ComponentModel.DataAnnotations;

namespace LearnHub.Back.Application.DTOs
{
    /// <summary>
    /// Represents a course in the system
    /// </summary>
    public class CourseDto
    {
        /// <summary>
        /// Unique identifier of the course
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the course
        /// </summary>
        [Required(ErrorMessage = "Course name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters")]
        public string Title { get; set; }

        /// <summary>
        /// Detailed description of the course
        /// </summary>
        [Required(ErrorMessage = "Course description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        /// <summary>
        /// Start date of the course
        /// </summary>
        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End date of the course
        /// </summary>
        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }

        public int Duration { get; set; }
        public decimal Price { get; set; }
        public string Prerequisites { get; set; }
        public Guid InstructorId { get; set; }
        public Instructor Instructor { get; set; }
        public string Modality { get; set; }
        public string IncludedMaterials { get; set; }
        public string Certification { get; set; }
        public int AvailableSeats { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }

        public List<EnrollmentDto> Enrollments { get; set; } = new List<EnrollmentDto>();
    }
}