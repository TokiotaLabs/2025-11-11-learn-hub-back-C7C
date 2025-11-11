using System;
using System.ComponentModel.DataAnnotations;

namespace LearnHub.Back.Application.DTOs
{
    /// <summary>
    /// Represents a course enrollment
    /// </summary>
    public class EnrollmentDto
    {
        /// <summary>
        /// Unique identifier of the enrollment
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// ID of the enrolled student
        /// </summary>
        [Required(ErrorMessage = "Student ID is required")]
        public Guid StudentId { get; set; }

        /// <summary>
        /// ID of the course being enrolled in
        /// </summary>
        [Required(ErrorMessage = "Course ID is required")]
        public Guid CourseId { get; set; }

        /// <summary>
        /// Date when the enrollment is made
        /// </summary>
        [Required(ErrorMessage = "Enrollment date is required")]
        public DateTime EnrollmentDate { get; set; }

        /// <summary>
        /// Current status of the enrollment
        /// </summary>
        [Required(ErrorMessage = "Status is required")]
        [RegularExpression("^(Approved|Rejected|Pending)$", 
            ErrorMessage = "Status must be 'Approved', 'Rejected' or 'Pending'")]
        public string Status { get; set; }

        /// <summary>
        /// Student's schedule preference
        /// </summary>
        [Required(ErrorMessage = "Schedule preference is required")]
        [StringLength(50, ErrorMessage = "Schedule preference cannot exceed 50 characters")]
        public string SchedulePreference { get; set; }

        /// <summary>
        /// ID of the payment associated with the enrollment
        /// </summary>
        public Guid? PaymentId { get; set; }

        // Navigation properties for the API
        public StudentDto Student { get; set; }
        public CourseDto Course { get; set; }
    }
}