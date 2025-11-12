using AutoMapper;
using FluentAssertions;
using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Application.Handlers.Course;
using LearnHub.Back.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Back.Tests.Application.Handlers.Course
{
    [TestFixture]
    public class CourseHandlerTests
    {
        private ApplicationDbContext _context;
        private IMapper _mapper;
        private DbContextOptions<ApplicationDbContext> _options;

        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(_options);
            _context.Database.EnsureCreated();

            var config = new MapperConfiguration(cfg => 
            {
                cfg.AddProfile<LearnHub.Back.Application.Mappings.CourseProfile>();
                cfg.AddProfile<LearnHub.Back.Application.Mappings.EnrollmentProfile>();
                cfg.AddProfile<LearnHub.Back.Application.Mappings.StudentProfile>();
            });
            
            _mapper = config.CreateMapper();
        }

        [Test]
        public async Task CreateCourse_WithValidData_ShouldCreateAndReturnDto()
        {
            // Arrange
            var command = new CreateCourseCommand
            {
                Title = "Test Course",
                Description = "Test Description",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(30),
                Duration = 5,
                Price = 99.99m,
                Prerequisites = "None",
                InstructorId = Guid.NewGuid(),
                Modality = "Online",
                IncludedMaterials = "None",
                Certification = "Certificate of Completion",
                AvailableSeats = 20,
                Location = "Online",
                Category = "Technology"
            };
            
            var handler = new CreateCourseCommandHandler(_mapper, _context);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Title.Should().Be(command.Title);
            result.Description.Should().Be(command.Description);
            result.Price.Should().Be(command.Price);
            result.Duration.Should().Be(command.Duration);
        }

        [Test]
        public async Task UpdateCourse_WithValidData_ShouldUpdateAndReturnUnit()
        {
            // Arrange
            var course = new Domain.Course
            {
                Title = "Original Title",
                Description = "Original Description",
                Price = 50m,
                Duration = 4,
                Category = "Sample Category",
                Certification = "Sample Certification",
                IncludedMaterials = "Sample Materials",
                Location = "Sample Location",
                Modality = "Sample Modality",
                Prerequisites = "Sample Prerequisites",
                InstructorId = Guid.NewGuid() // Assuming you have a valid InstructorId
            };
            
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            var command = new UpdateCourseCommand
            {
                Id = course.Id,
                Title = "Updated Title",
                Description = "Updated Description",
                Price = 75m,
                Duration = 2
            };

            var handler = new UpdateCourseCommandHandler(_mapper, _context);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var updatedCourse = await _context.Courses.FindAsync(course.Id);
            updatedCourse.Should().NotBeNull();
            updatedCourse.Title.Should().Be(command.Title);
            updatedCourse.Description.Should().Be(command.Description);
            updatedCourse.Price.Should().Be(command.Price);
            updatedCourse.Duration.Should().Be(command.Duration);
        }

        [Test]
        public async Task DeleteCourse_WithExistingId_ShouldRemoveAndSaveChanges()
        {
            // Arrange
            var course = new Domain.Course
            {
                Title = "Test Course",
                Description = "Test Description",
                Price = 99.99m,
                Duration = 2,
                Prerequisites = "None",
                Modality = "Online",
                IncludedMaterials = "None",
                Certification = "None",
                Location = "Test Location",
                Category = "Test Category"
            };
            
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            var handler = new DeleteCourseCommandHandler(_context);
            var command = new DeleteCourseCommand { Id = course.Id };

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var deletedCourse = await _context.Courses.FindAsync(course.Id);
            deletedCourse.Should().BeNull();
        }

        [Test]
        public async Task DeleteCourse_WithNonExistingId_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var handler = new DeleteCourseCommandHandler(_context);
            var command = new DeleteCourseCommand { Id = Guid.NewGuid() };

            // Act & Assert
            await handler.Invoking(h => h.Handle(command, CancellationToken.None))
                .Should().ThrowAsync<KeyNotFoundException>();
        }

        [Test]
        public async Task GetMostDemandedCourses_ShouldReturnCoursesOrderedByEnrollmentCount()
        {
            // Arrange
            var instructor = new Domain.Instructor
            {
                Name = "Test Instructor",
                Biography = "Test Biography"
            };
            _context.Instructors.Add(instructor);
            await _context.SaveChangesAsync();

            var course1 = new Domain.Course
            {
                Title = "Course 1",
                Description = "Description 1",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(30),
                Price = 50m,
                Duration = 4,
                Category = "Category 1",
                Certification = "Cert 1",
                IncludedMaterials = "Materials 1",
                Location = "Location 1",
                Modality = "Online",
                Prerequisites = "None",
                InstructorId = instructor.Id
            };

            var course2 = new Domain.Course
            {
                Title = "Course 2",
                Description = "Description 2",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(30),
                Price = 60m,
                Duration = 5,
                Category = "Category 2",
                Certification = "Cert 2",
                IncludedMaterials = "Materials 2",
                Location = "Location 2",
                Modality = "Online",
                Prerequisites = "None",
                InstructorId = instructor.Id
            };

            var course3 = new Domain.Course
            {
                Title = "Course 3",
                Description = "Description 3",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(30),
                Price = 70m,
                Duration = 6,
                Category = "Category 3",
                Certification = "Cert 3",
                IncludedMaterials = "Materials 3",
                Location = "Location 3",
                Modality = "Online",
                Prerequisites = "None",
                InstructorId = instructor.Id
            };

            _context.Courses.AddRange(course1, course2, course3);
            await _context.SaveChangesAsync();

            // Add enrollments - course2 has most, then course3, then course1
            _context.Enrollments.AddRange(
                new Domain.Enrollment { CourseId = course1.Id, StudentId = Guid.NewGuid(), Status = "Approved", EnrollmentDate = DateTime.UtcNow, SchedulePreference = "Morning" },
                new Domain.Enrollment { CourseId = course2.Id, StudentId = Guid.NewGuid(), Status = "Approved", EnrollmentDate = DateTime.UtcNow, SchedulePreference = "Morning" },
                new Domain.Enrollment { CourseId = course2.Id, StudentId = Guid.NewGuid(), Status = "Approved", EnrollmentDate = DateTime.UtcNow, SchedulePreference = "Morning" },
                new Domain.Enrollment { CourseId = course2.Id, StudentId = Guid.NewGuid(), Status = "Approved", EnrollmentDate = DateTime.UtcNow, SchedulePreference = "Morning" },
                new Domain.Enrollment { CourseId = course3.Id, StudentId = Guid.NewGuid(), Status = "Approved", EnrollmentDate = DateTime.UtcNow, SchedulePreference = "Morning" },
                new Domain.Enrollment { CourseId = course3.Id, StudentId = Guid.NewGuid(), Status = "Approved", EnrollmentDate = DateTime.UtcNow, SchedulePreference = "Morning" }
            );
            await _context.SaveChangesAsync();

            // Verify data is saved
            var savedCourses = await _context.Courses.Include(c => c.Enrollments).ToListAsync();
            savedCourses.Should().HaveCount(3);

            var handler = new GetMostDemandedCoursesQueryHandler(_context, _mapper);
            var query = new GetMostDemandedCoursesQuery { Limit = 10 };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
            result[0].Title.Should().Be("Course 2"); // 3 enrollments
            result[1].Title.Should().Be("Course 3"); // 2 enrollments
            result[2].Title.Should().Be("Course 1"); // 1 enrollment
        }

        [Test]
        public async Task GetMostDemandedCourses_WithLimit_ShouldReturnLimitedResults()
        {
            // Arrange
            var instructor = new Domain.Instructor
            {
                Name = "Test Instructor",
                Biography = "Test Biography"
            };
            _context.Instructors.Add(instructor);
            await _context.SaveChangesAsync();

            var course1 = new Domain.Course
            {
                Title = "Course 1",
                Description = "Description 1",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(30),
                Price = 50m,
                Duration = 4,
                Category = "Category 1",
                Certification = "Cert 1",
                IncludedMaterials = "Materials 1",
                Location = "Location 1",
                Modality = "Online",
                Prerequisites = "None",
                InstructorId = instructor.Id
            };

            var course2 = new Domain.Course
            {
                Title = "Course 2",
                Description = "Description 2",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(30),
                Price = 60m,
                Duration = 5,
                Category = "Category 2",
                Certification = "Cert 2",
                IncludedMaterials = "Materials 2",
                Location = "Location 2",
                Modality = "Online",
                Prerequisites = "None",
                InstructorId = instructor.Id
            };

            var course3 = new Domain.Course
            {
                Title = "Course 3",
                Description = "Description 3",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(30),
                Price = 70m,
                Duration = 6,
                Category = "Category 3",
                Certification = "Cert 3",
                IncludedMaterials = "Materials 3",
                Location = "Location 3",
                Modality = "Online",
                Prerequisites = "None",
                InstructorId = instructor.Id
            };

            _context.Courses.AddRange(course1, course2, course3);
            await _context.SaveChangesAsync();

            // Add enrollments - course2 has 3, course3 has 2, course1 has 1
            _context.Enrollments.AddRange(
                new Domain.Enrollment { CourseId = course1.Id, StudentId = Guid.NewGuid(), Status = "Approved", EnrollmentDate = DateTime.UtcNow, SchedulePreference = "Morning" },
                new Domain.Enrollment { CourseId = course2.Id, StudentId = Guid.NewGuid(), Status = "Approved", EnrollmentDate = DateTime.UtcNow, SchedulePreference = "Morning" },
                new Domain.Enrollment { CourseId = course2.Id, StudentId = Guid.NewGuid(), Status = "Approved", EnrollmentDate = DateTime.UtcNow, SchedulePreference = "Morning" },
                new Domain.Enrollment { CourseId = course2.Id, StudentId = Guid.NewGuid(), Status = "Approved", EnrollmentDate = DateTime.UtcNow, SchedulePreference = "Morning" },
                new Domain.Enrollment { CourseId = course3.Id, StudentId = Guid.NewGuid(), Status = "Approved", EnrollmentDate = DateTime.UtcNow, SchedulePreference = "Morning" },
                new Domain.Enrollment { CourseId = course3.Id, StudentId = Guid.NewGuid(), Status = "Approved", EnrollmentDate = DateTime.UtcNow, SchedulePreference = "Morning" }
            );
            await _context.SaveChangesAsync();

            var handler = new GetMostDemandedCoursesQueryHandler(_context, _mapper);
            var query = new GetMostDemandedCoursesQuery { Limit = 2 };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result[0].Title.Should().Be("Course 2"); // 3 enrollments
            result[1].Title.Should().Be("Course 3"); // 2 enrollments
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}