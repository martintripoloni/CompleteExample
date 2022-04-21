using NUnit.Framework;
using System.Threading.Tasks;
using CompleteExample.Logic.Validators;
using CompleteExample.Entities;
using FluentValidation.TestHelper;
using CompleteExample.Logic.Queries;
using System.Linq;

namespace CompleteExample.Logic.Tests
{
    public class EnrollStudentCourseCommandArgumentsValidatorTest
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
        public async Task Course_And_Student_Exist_No_Errors_Are_Shown()
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

            await context.SaveChangesAsync();

            // Act
            var result = new EnrollStudentCourseCommandArgumentsValidator(context).TestValidate(args);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public async Task Course_Not_Exist_Error_Is_Shown()
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

            var course = new Course() { CourseId = 51, InstructorId = instructor.InstructorId, Credits = 20 };
            context.Courses.Add(course);

            var student = new Student() { StudentId = 2 };
            context.Students.Add(student);

            await context.SaveChangesAsync();

            // Act
            var result = new EnrollStudentCourseCommandArgumentsValidator(context).TestValidate(args);

            // Assert
            Assert.IsTrue(result.Errors.Any());
            result.ShouldHaveValidationErrorFor(x => x.CourseId).WithErrorMessage("Course must exist.");
        }

        [Test]
        public async Task Student_Not_Exist_Error_Is_Shown()
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

            var student = new Student() { StudentId = 3 };
            context.Students.Add(student);

            await context.SaveChangesAsync();

            // Act
            var result = new EnrollStudentCourseCommandArgumentsValidator(context).TestValidate(args);

            // Assert
            Assert.IsTrue(result.Errors.Any());
            result.ShouldHaveValidationErrorFor(x => x.StudentId).WithErrorMessage("Student must exist.");
        }

        [Test]
        public async Task Enrol_Not_Exist_Error_Is_Shown()
        {
            // Arrange
            var args = new EnrollStudentCourseCommand.Arguments()
            {
                CourseId = 50,
                StudentId = 3
            };

            using var factory = new SampleDbContextFactory();
            using var context = factory.CreateContext();

            var instructor = new Instructor() { InstructorId = 1, Email = "test@sample.com" };
            context.Instructors.Add(instructor);

            var course = new Course() { CourseId = 50, InstructorId = instructor.InstructorId, Credits = 20 };
            context.Courses.Add(course);

            var student = new Student() { StudentId = 3 };
            context.Students.Add(student);

            var enrollment = new Enrollment() { StudentId = 3, CourseId = 50 };
            context.Enrollment.Add(enrollment);

            await context.SaveChangesAsync();

            // Act
            var result = new EnrollStudentCourseCommandArgumentsValidator(context).TestValidate(args);

            // Assert
            Assert.IsTrue(result.Errors.Any());
            result.ShouldHaveValidationErrorFor(x => x).WithErrorMessage("The student is already registered for that course.");
        }
    }
}