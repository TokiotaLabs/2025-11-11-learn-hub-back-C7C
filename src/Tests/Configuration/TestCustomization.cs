using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using LearnHub.Back.Application.Handlers.Course;
using LearnHub.Back.Application.Handlers.Enrollment;
using LearnHub.Back.Domain;

namespace LearnHub.Back.Tests.Configuration;

public class TestCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize(new AutoMoqCustomization());
        
        // Handle circular references
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        // Customize BindingInfo creation
        fixture.Customize<BindingInfo>(composer => composer
            .Without(b => b.BinderType));

        // Configure valid course dates and data
        fixture.Customize<Course>(composer => composer
            .With(x => x.StartDate, DateTime.Now.AddDays(1))
            .With(x => x.EndDate, DateTime.Now.AddDays(30))
            .With(x => x.Instructor, fixture.Create<Instructor>())
            .With(x => x.Title, "Test Course")
            .With(x => x.Description, "Test Description")
            .With(x => x.Price, 100m)
            .With(x => x.Duration, 30)
            .With(x => x.Prerequisites, "None")
            .With(x => x.Modality, "Online")
            .With(x => x.IncludedMaterials, "Course Materials")
            .With(x => x.Certification, "Course Certificate")
            .With(x => x.AvailableSeats, 20)
            .With(x => x.Location, "Online")
            .With(x => x.Category, "Programming"));

        // Configure instructor with required properties
        fixture.Customize<Instructor>(composer => composer
            .With(x => x.Biography, "Test instructor biography")
            .With(x => x.Name, "Test Instructor"));

        // Configure student with required properties
        fixture.Customize<Student>(composer => composer
            .With(x => x.FullName, "Test Student")
            .With(x => x.Email, "test@example.com")
            .With(x => x.PhoneNumber, "1234567890")
            .With(x => x.PostalAddress, "123 Test St")
            .With(x => x.EducationLevel, "Bachelor")
            .With(x => x.CurrentOccupation, "Student")
            .With(x => x.PreviousExperience, "None"));

        // Configure enrollment with required properties
        fixture.Customize<Enrollment>(composer => composer
            .With(x => x.Status, "Pending")
            .With(x => x.EnrollmentDate, DateTime.UtcNow)
            .With(x => x.SchedulePreference, "Morning")
            .Without(x => x.Payment));  // Payment is optional

        // Configure enrollment commands
        fixture.Customize<CreateEnrollmentCommand>(composer => composer
            .With(x => x.Status, "Pending")
            .With(x => x.SchedulePreference, "Morning"));

        fixture.Customize<UpdateEnrollmentCommand>(composer => composer
            .With(x => x.Status, "Pending")
            .With(x => x.SchedulePreference, "Morning"));

        // Configure course commands
        fixture.Customize<CreateCourseCommand>(composer => composer
            .With(x => x.StartDate, DateTime.Now.AddDays(1))
            .With(x => x.EndDate, DateTime.Now.AddDays(30))
            .With(x => x.Title, "Test Course")
            .With(x => x.Description, "Test Description")
            .With(x => x.Price, 100m)
            .With(x => x.Duration, 30)
            .With(x => x.Prerequisites, "None")
            .With(x => x.Modality, "Online")
            .With(x => x.IncludedMaterials, "Course Materials")
            .With(x => x.Certification, "Course Certificate")
            .With(x => x.AvailableSeats, 20)
            .With(x => x.Location, "Online")
            .With(x => x.Category, "Programming"));

        fixture.Customize<UpdateCourseCommand>(composer => composer
            .With(x => x.StartDate, DateTime.Now.AddDays(1))
            .With(x => x.EndDate, DateTime.Now.AddDays(30))
            .With(x => x.Title, "Test Course")
            .With(x => x.Description, "Test Description")
            .With(x => x.Price, 100m)
            .With(x => x.Duration, 30)
            .With(x => x.Prerequisites, "None")
            .With(x => x.Modality, "Online")
            .With(x => x.IncludedMaterials, "Course Materials")
            .With(x => x.Certification, "Course Certificate")
            .With(x => x.AvailableSeats, 20)
            .With(x => x.Location, "Online")
            .With(x => x.Category, "Programming"));
    }
}