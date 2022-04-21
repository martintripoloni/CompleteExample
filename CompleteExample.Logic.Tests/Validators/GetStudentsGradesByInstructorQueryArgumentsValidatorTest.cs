using NUnit.Framework;
using System.Threading.Tasks;
using CompleteExample.Logic.Validators;
using Moq;
using CompleteExample.Entities;
using FluentValidation.TestHelper;
using CompleteExample.Logic.Queries;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Data.Entity;

namespace CompleteExample.Logic.Tests
{
    public class GetStudentsGradesByInstructorQueryArgumentsValidatorTest
    {
        private GetStudentsGradesByInstructorQueryArgumentsValidator cut;

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {

        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Test()
        {
            // Arrange
            var completeExampleDBContext = new Mock<ICompleteExampleDBContext>();

            var data = new List<Instructor>
            {
                new Instructor { InstructorId = 1 },
                new Instructor { InstructorId = 2 },
                new Instructor { InstructorId = 3 },
            }.AsQueryable();
                       
            var args = new GetStudentsGradesByInstructorQuery.Arguments() { };

            // Act
            var result = new GetStudentsGradesByInstructorQueryArgumentsValidator(completeExampleDBContext.Object).TestValidate(args);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}