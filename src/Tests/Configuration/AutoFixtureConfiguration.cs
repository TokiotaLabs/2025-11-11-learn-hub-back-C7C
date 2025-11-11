using AutoFixture;
using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using LearnHub.Back.Application.Handlers.Course;
using LearnHub.Back.Domain;
using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Application.Handlers.Enrollment;
using System.Linq;

namespace LearnHub.Back.Tests.Configuration
{
    public class AutoFixtureConfiguration
    {
        public static Fixture Create()
        {
            var fixture = new Fixture();
            
            // Handle circular references
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            
            // Handle model binding issues
            fixture.Customize<BindingInfo>(composer => composer
                .Without(p => p.BinderType));

            // Ensure course dates are valid
            fixture.Customize<CreateCourseCommand>(composer =>
                composer.With(x => x.StartDate, DateTime.Now.AddDays(1))
                       .With(x => x.EndDate, DateTime.Now.AddDays(30)));

            fixture.Customize<UpdateCourseCommand>(composer =>
                composer.With(x => x.StartDate, DateTime.Now.AddDays(1))
                       .With(x => x.EndDate, DateTime.Now.AddDays(30)));

            // Ensure Course has valid properties
            fixture.Customize<Course>(composer =>
                composer.With(x => x.StartDate, DateTime.Now.AddDays(1))
                       .With(x => x.EndDate, DateTime.Now.AddDays(30))
                       .Without(x => x.Instructor)); // Prevent circular reference

            // Ensure Instructor has required properties
            fixture.Customize<Instructor>(composer =>
                composer.With(x => x.Biography, "Test instructor biography")
                       .With(x => x.Name, "Test Instructor")
                       .Without(x => x.Courses)); // Prevent circular reference

            // Configure enrollments with required properties
            fixture.Customize<Enrollment>(composer =>
                composer.With(x => x.Status, "Pending")
                       .With(x => x.EnrollmentDate, DateTime.Now)
                       .With(x => x.SchedulePreference, "Morning"));

            fixture.Customize<EnrollmentDto>(composer =>
                composer.With(x => x.Status, "Pending")
                       .With(x => x.EnrollmentDate, DateTime.Now)
                       .With(x => x.SchedulePreference, "Morning"));

            fixture.Customize<CreateEnrollmentCommand>(composer =>
                composer.With(x => x.Status, "Pending")
                       .With(x => x.SchedulePreference, "Morning"));

            fixture.Customize<UpdateEnrollmentCommand>(composer =>
                composer.With(x => x.Status, "Pending")
                       .With(x => x.SchedulePreference, "Morning"));

            return fixture;
        }
    }

    public class ApiAutoDataAttribute : AutoDataAttribute
    {
        public ApiAutoDataAttribute() : base(() => AutoFixtureConfiguration.Create())
        {
        }
    }
}