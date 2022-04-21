using NUnit.Framework;
using System.Threading.Tasks;
using CompleteExample.Logic.Validators;
using CompleteExample.Entities;
using FluentValidation.TestHelper;
using CompleteExample.Logic.Queries;
using System.Linq;

namespace CompleteExample.Logic.Tests
{
    public class UpdateStudentGradeCommandArgumentsValidatorTest
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
        public async Task All_Data_Is_Valid_No_Errors_Are_Shown()
        {
            // Arrange
            var args = new UpdateStudentGradeCommand.Arguments()
            {
                CourseId = 50,
                StudentId = 2,
                Grade = 100,
            };

            using var factory = new SampleDbContextFactory();
            using var context = factory.CreateContext();

            var instructor = new Instructor() { InstructorId = 1, Email = "test@sample.com" };
            context.Instructors.Add(instructor);

            var course = new Course() { CourseId = 50, InstructorId = instructor.InstructorId, Credits = 20 };
            context.Courses.Add(course);

            var student = new Student() { StudentId = 2 };
            context.Students.Add(student);

            var enrollment = new Enrollment() { StudentId = 2, CourseId = 50 };
            context.Enrollment.Add(enrollment);

            await context.SaveChangesAsync();

            // Act
            var result = new UpdateStudentGradeCommandArgumentsValidator(context).TestValidate(args);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public async Task Course_Not_Exist_Error_Is_Shown()
        {
            // Arrange
            var args = new UpdateStudentGradeCommand.Arguments()
            {
                CourseId = 50,
                StudentId = 2,
                Grade = 100,
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
            var result = new UpdateStudentGradeCommandArgumentsValidator(context).TestValidate(args);

            // Assert
            Assert.IsTrue(result.Errors.Any());
            result.ShouldHaveValidationErrorFor(x => x.CourseId).WithErrorMessage("Course must exist.");
        }

        [Test]
        public async Task Student_Not_Exist_Error_Is_Shown()
        {
            // Arrange
            var args = new UpdateStudentGradeCommand.Arguments()
            {
                CourseId = 50,
                StudentId = 2,
                Grade = 100,
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
            var result = new UpdateStudentGradeCommandArgumentsValidator(context).TestValidate(args);

            // Assert
            Assert.IsTrue(result.Errors.Any());
            result.ShouldHaveValidationErrorFor(x => x.StudentId).WithErrorMessage("Student must exist.");
        }

        [Test]
        public async Task Student_Is_Not_Registered_In_Course_Error_Is_Shown()
        {
            // Arrange
            var args = new UpdateStudentGradeCommand.Arguments()
            {
                CourseId = 50,
                StudentId = 3,
                Grade = 100,
            };

            using var factory = new SampleDbContextFactory();
            using var context = factory.CreateContext();

            var instructor = new Instructor() { InstructorId = 1, Email = "test@sample.com" };
            context.Instructors.Add(instructor);

            var course = new Course() { CourseId = 51, InstructorId = instructor.InstructorId, Credits = 20 };
            context.Courses.Add(course);

            var student = new Student() { StudentId = 3 };
            context.Students.Add(student);

            var otherStudent = new Student() { StudentId = 4 };
            context.Students.Add(otherStudent);

            var enrollment = new Enrollment() { StudentId = 4, CourseId = 51 };
            context.Enrollment.Add(enrollment);

            await context.SaveChangesAsync();

            // Act
            var result = new UpdateStudentGradeCommandArgumentsValidator(context).TestValidate(args);

            // Assert
            Assert.IsTrue(result.Errors.Any());
            result.ShouldHaveValidationErrorFor(x => x).WithErrorMessage("The student is not registered for that course.");
        }
    }
}