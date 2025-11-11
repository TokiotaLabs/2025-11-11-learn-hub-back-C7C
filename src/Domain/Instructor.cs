namespace LearnHub.Back.Domain;

public class Instructor
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Biography { get; set; }

    public List<Course> Courses { get; set; } = new List<Course>();
}