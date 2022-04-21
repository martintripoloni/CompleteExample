using CompleteExample.Entities;
using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using CompleteExample.Logic.Queries;
using System.Threading;
using System.Linq;

namespace CompleteExample.Logic.Tests.Commands
{
    public class EnrollStudentCourseCommandTest
    {
        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Arguments_Are_Valid_Enrollment_Is_Created_And_Returned_Id()
        {
            // Arrange
            var args = new EnrollStudentCourseCommand.Arguments()
            {
                CourseId = 50,
                StudentId = 2
            };

            using var factory = new SampleDbContextFactory();
            using var context = factory.CreateContext();

            var instructor = new Instructor() { InstructorId = 1, Email = "test@sample.com" };
            context.Instructors.Add(instructor);

            var course = new Course() { CourseId = 50, InstructorId = instructor.InstructorId, Credits = 20 };
            context.Courses.Add(course);

            var student = new Student() { StudentId = 2 };
            context.Students.Add(student);

            var otherStudent = new Student() { StudentId = 4 };
            context.Students.Add(otherStudent);

            var enrollment = new Enrollment() { StudentId = 4, CourseId = 50 };
            context.Enrollment.Add(enrollment);

            await context.SaveChangesAsync();

            // Act
            var result = new EnrollStudentCourseCommand.EnrollStudentCourseCommandHandler(context).Handle(args, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsTrue(context.Enrollment.Count() == 2);
            Assert.IsNotNull(context.Enrollment.FirstOrDefault(x => x.CourseId == 50 && x.StudentId == 2));
        }
    }
}
