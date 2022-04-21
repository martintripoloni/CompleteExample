using NUnit.Framework;
using System.Threading.Tasks;
using CompleteExample.Logic.Validators;
using CompleteExample.Entities;
using FluentValidation.TestHelper;
using CompleteExample.Logic.Queries;
using System.Linq;

namespace CompleteExample.Logic.Tests
{
    public class GetStudentsGradesByInstructorQueryArgumentsValidatorTest
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
        public async Task Instructor_Exist_No_Errors_Are_Shown()
        {
            // Arrange
            var args = new GetStudentsGradesByInstructorQuery.Arguments()
            {
                InstructorId = 50,
            };

            using var factory = new SampleDbContextFactory();
            using var context = factory.CreateContext();
            var instructor = new Instructor() { InstructorId = 50, Email = "test@sample.com" };
            context.Instructors.Add(instructor);
            await context.SaveChangesAsync();

            // Act
            var result = new GetStudentsGradesByInstructorQueryArgumentsValidator(context).TestValidate(args);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public async Task Instructor_Not_Exist_Error_Is_Shown()
        {
            // Arrange
            var args = new GetStudentsGradesByInstructorQuery.Arguments()
            {
                InstructorId = 40,
            };

            using var factory = new SampleDbContextFactory();
            using var context = factory.CreateContext();
            var instructor = new Instructor() { InstructorId = 50, Email = "test@sample.com" };
            context.Instructors.Add(instructor);
            await context.SaveChangesAsync();

            // Act
            var result = new GetStudentsGradesByInstructorQueryArgumentsValidator(context).TestValidate(args);

            // Assert
            Assert.IsTrue(result.Errors.Any());
            result.ShouldHaveValidationErrorFor(x => x.InstructorId).WithErrorMessage("Instructor must exist.");
        }
    }
}