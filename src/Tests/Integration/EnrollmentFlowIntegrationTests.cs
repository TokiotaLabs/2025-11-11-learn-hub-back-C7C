using FluentAssertions;
using LearnHub.Back.Domain;
using LearnHub.Back.Infrastructure;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace LearnHub.Back.Tests.Integration;

[TestFixture]
public class EnrollmentFlowIntegrationTests
{
    private DbContextOptions<ApplicationDbContext> _options;
    private string _databaseName;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _databaseName = $"LearnHubTestDb_{Guid.NewGuid()}";
    }

    [SetUp]
    public void Setup()
    {
        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: _databaseName)
            .Options;
    }

    [TearDown]
    public async Task TearDown()
    {
        using var context = new ApplicationDbContext(_options);
        await context.Database.EnsureDeletedAsync();
    }

    [Test]
    public async Task CompleteEnrollmentFlow_ShouldWorkCorrectly()
    {
        // Arrange
        var instructor = new Instructor { 
            Name = "John",
            Biography = "Experienced instructor" 
        };
        var student = new Student { 
            FullName = "Jane Smith", 
            Email = "jane@example.com",
            PhoneNumber = "+1234567890",
            PostalAddress = "123 Main St",
            CurrentOccupation = "Software Developer",
            EducationLevel = "Bachelor's Degree",
            PreviousExperience = "3 years of programming"
        };
        var course = new Course 
        { 
            Title = "Test Course",
            Description = "Test Description",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddMonths(1),
            Duration = 40,
            Price = 100,
            Prerequisites = "None",
            InstructorId = instructor.Id,
            Instructor = instructor,
            Modality = "Online",
            IncludedMaterials = "Course materials",
            Certification = "Course certificate",
            AvailableSeats = 10,
            Location = "Virtual",
            Category = "Programming"
        };

        using (var context = new ApplicationDbContext(_options))
        {
            await context.Database.EnsureCreatedAsync();
            await context.Instructors.AddAsync(instructor);
            await context.Courses.AddAsync(course);
            await context.Students.AddAsync(student);
            await context.SaveChangesAsync();
        }

        // Act & Assert
        using (var context = new ApplicationDbContext(_options))
        {
            var enrollment = new Enrollment
            {
                StudentId = student.Id,
                CourseId = course.Id,
                Status = "Pending",
                EnrollmentDate = DateTime.UtcNow,
                SchedulePreference = "Morning"
            };

            await context.Enrollments.AddAsync(enrollment);
            await context.SaveChangesAsync();

            var savedEnrollment = await context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.Id == enrollment.Id);

            savedEnrollment.Should().NotBeNull();
            savedEnrollment!.Student.FullName.Should().Be("Jane Smith");
            savedEnrollment.Course.Title.Should().Be("Test Course");
            
            savedEnrollment.Status = "Approved";
            await context.SaveChangesAsync();

            var updatedEnrollment = await context.Enrollments.FindAsync(enrollment.Id);
            updatedEnrollment!.Status.Should().Be("Approved");
        }
    }

    [Test]
    public async Task EnrollmentFlow_WithFullCourse_ShouldKeepPendingStatus()
    {
        // Arrange
        using var context = new ApplicationDbContext(_options);
        await context.Database.EnsureCreatedAsync();

        var instructor = new Instructor { 
            Name = "Alice",
            Biography = "Expert instructor"
        };
        var student = new Student { 
            FullName = "Bob Wilson", 
            Email = "bob@example.com",
            PhoneNumber = "+1987654321",
            PostalAddress = "456 Oak St",
            CurrentOccupation = "Student",
            EducationLevel = "High School",
            PreviousExperience = "None"
        };
        var course = new Course
        {
            Title = "Full Course",
            Description = "No seats available",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddMonths(1),
            Duration = 40,
            Price = 100,
            Prerequisites = "None",
            InstructorId = instructor.Id,
            Instructor = instructor,
            Modality = "Online",
            IncludedMaterials = "Course materials",
            Certification = "Course certificate",
            AvailableSeats = 0,
            Location = "Virtual",
            Category = "Programming"
        };

        await context.Instructors.AddAsync(instructor);
        await context.Courses.AddAsync(course);
        await context.Students.AddAsync(student);
        await context.SaveChangesAsync();

        var enrollment = new Enrollment
        {
            StudentId = student.Id,
            CourseId = course.Id,
            Status = "Pending",
            EnrollmentDate = DateTime.UtcNow,
            SchedulePreference = "Morning"
        };

        await context.Enrollments.AddAsync(enrollment);
        await context.SaveChangesAsync();

        var savedEnrollment = await context.Enrollments
            .Include(e => e.Course)
            .FirstOrDefaultAsync(e => e.Id == enrollment.Id);

        savedEnrollment.Should().NotBeNull();
        savedEnrollment!.Course.AvailableSeats.Should().Be(0);
        savedEnrollment.Status.Should().Be("Pending");
    }
}