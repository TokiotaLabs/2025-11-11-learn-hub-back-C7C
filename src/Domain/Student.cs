namespace LearnHub.Back.Domain;

public class Student
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string PostalAddress { get; set; }
    public string EducationLevel { get; set; }
    public string CurrentOccupation { get; set; }
    public string PreviousExperience { get; set; }

    public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}