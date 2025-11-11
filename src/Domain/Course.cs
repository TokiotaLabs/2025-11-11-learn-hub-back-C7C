namespace LearnHub.Back.Domain;

public class Course
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Duration { get; set; } // in hours or weeks
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
    public List<string> Schedule { get; set; } = new List<string>();
    public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}