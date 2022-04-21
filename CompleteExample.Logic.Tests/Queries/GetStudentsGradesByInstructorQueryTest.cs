using CompleteExample.Entities;
using NUnit.Framework;
using System.Threading.Tasks;
using CompleteExample.Logic.Queries;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

namespace CompleteExample.Logic.Tests.Queries
{
    public class GetStudentsGradesByInstructorQueryTest
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
        public async Task Arguments_Are_Valid_Students_Grades_By_Instructor_Are_Returned()
        {
            // Arrange
            var args = new GetStudentsGradesByInstructorQuery.Arguments()
            {
                InstructorId = 2,
            };

            using var factory = new SampleDbContextFactory();
            using var context = factory.CreateContext();

            var instructor = new Instructor() { InstructorId = 2, Email = "test@sample.com" };
            context.Instructors.Add(instructor);


            context.Courses.AddRange(new List<Course>(){
                new Course() { CourseId = 50, Title = "JS 2022", InstructorId = instructor.InstructorId, Credits = 20 },
                new Course() { CourseId = 55, Title = ".NET 2022", InstructorId = instructor.InstructorId, Credits = 50 },
            });

            context.Students.AddRange(new List<Student>(){
                new Student() { StudentId = 2, FirstName = "Aubrie", LastName = "McCaw" },
                new Student() { StudentId = 4, FirstName = "Findley", LastName = "Anster" },
                new Student() { StudentId = 6, FirstName = "Jedidiah", LastName = "Stegers" },
            });

            context.Enrollment.AddRange(new List<Enrollment>(){
                new Enrollment() { EnrollmentId = 1, StudentId = 2, CourseId = 50, Grade = 10 },
                new Enrollment() { EnrollmentId = 2, StudentId = 4, CourseId = 50, Grade = 15 },
                new Enrollment() { EnrollmentId = 3, StudentId = 6, CourseId = 50, Grade = 20 },

                new Enrollment() { EnrollmentId = 4, StudentId = 2, CourseId = 55, Grade = 60 },
                new Enrollment() { EnrollmentId = 5, StudentId = 4, CourseId = 55, Grade = 80 },
            });

            await context.SaveChangesAsync();

            context.EnrollmentHistories.AddRange(new List<EnrollmentHistory>(){
                new EnrollmentHistory() { EnrollmentId = 1, StudentId = 2, CourseId = 50 },
                new EnrollmentHistory() { EnrollmentId = 2, StudentId = 4, CourseId = 50 },
                new EnrollmentHistory() { EnrollmentId = 3, StudentId = 6, CourseId = 50 },

                new EnrollmentHistory() { EnrollmentId = 4, StudentId = 2, CourseId = 55 },
                new EnrollmentHistory() { EnrollmentId = 5, StudentId = 4, CourseId = 55 },

                new EnrollmentHistory() { EnrollmentId = 2, StudentId = 4, CourseId = 50, Grade = 5.5m },
                new EnrollmentHistory() { EnrollmentId = 2, StudentId = 4, CourseId = 50, Grade = 3.5m },
                new EnrollmentHistory() { EnrollmentId = 3, StudentId = 6, CourseId = 50, Grade = 10m },
            });

            await context.SaveChangesAsync();

            // Act
            var result = new GetStudentsGradesByInstructorQuery.GetStudentsGradesByInstructorQueryHandler(context).Handle(args, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsTrue(result.Result.StudentsGrades.Count() == 5);
            Assert.IsTrue(result.Result.StudentsGrades.Count(x => x.Course == "JS 2022") == 3);
            Assert.IsTrue(result.Result.StudentsGrades.Count(x => x.Course == ".NET 2022") == 2);
            Assert.IsTrue(result.Result.StudentsGrades.Where(x => x.Course == "JS 2022" && x.FirstName == "Aubrie" && x.LastName == "McCaw").Select(x => x.Grades).Count() == 1);
            Assert.IsTrue(result.Result.StudentsGrades.Where(x => x.Course == "JS 2022" && x.FirstName == "Findley" && x.LastName == "Anster").SelectMany(x => x.Grades).Select(g => g).Count() == 3);
            Assert.IsTrue(result.Result.StudentsGrades.Where(x => x.Course == "JS 2022" && x.FirstName == "Jedidiah" && x.LastName == "Stegers").SelectMany(x => x.Grades).Select(g => g).Count() == 2);
            Assert.IsTrue(result.Result.StudentsGrades.Where(x => x.Course == ".NET 2022" && x.FirstName == "Aubrie" && x.LastName == "McCaw").SelectMany(x => x.Grades).Select(g => g).Count() == 1);
            Assert.IsTrue(result.Result.StudentsGrades.First(x => x.Course == ".NET 2022" && x.FirstName == "Aubrie" && x.LastName == "McCaw").Grades.First() == 60m);
            Assert.IsTrue(result.Result.StudentsGrades.First(x => x.Course == ".NET 2022" && x.FirstName == "Findley" && x.LastName == "Anster").Grades.First() == 80m);

        }
    }
}
