using CompleteExample.Entities;
using NUnit.Framework;
using System.Threading.Tasks;
using CompleteExample.Logic.Queries;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

namespace CompleteExample.Logic.Tests.Queries
{
    public class GetTopGradesStudentsByCourseQueryTest
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
            var args = new GetTopGradesStudentsByCourseQuery.Arguments()
            {
            };

            using var factory = new SampleDbContextFactory();
            using var context = factory.CreateContext();

            var instructor = new Instructor() { InstructorId = 2, Email = "test@sample.com" };
            context.Instructors.Add(instructor);


            context.Courses.AddRange(new List<Course>(){
                new Course() { CourseId = 50, Title = "JS 2022", InstructorId = instructor.InstructorId, Credits = 20 },
                new Course() { CourseId = 55, Title = ".NET 2022", InstructorId = instructor.InstructorId, Credits = 50 },
                new Course() { CourseId = 60, Title = "CSS 2022", InstructorId = instructor.InstructorId, Credits = 50 },
            });

            context.Students.AddRange(new List<Student>(){
                new Student() { StudentId = 2, FirstName = "Aubrie", LastName = "McCaw" },
                new Student() { StudentId = 4, FirstName = "Findley", LastName = "Anster" },
                new Student() { StudentId = 6, FirstName = "Jedidiah", LastName = "Stegers" },
                new Student() { StudentId = 8, FirstName = "Abbot", LastName = "Le Strange" },
                new Student() { StudentId = 10, FirstName = "Jammal", LastName = "Darkin" },
                new Student() { StudentId = 12, FirstName = "Talyah", LastName = "Papes" },
            });

            context.Enrollment.AddRange(new List<Enrollment>(){
                new Enrollment() { EnrollmentId = 1, StudentId = 2, CourseId = 50, Grade = 10 },
                new Enrollment() { EnrollmentId = 2, StudentId = 4, CourseId = 50, Grade = 15 },
                new Enrollment() { EnrollmentId = 3, StudentId = 6, CourseId = 50, Grade = 20 },
                new Enrollment() { EnrollmentId = 8, StudentId = 8, CourseId = 50, Grade = 5 },

                new Enrollment() { EnrollmentId = 4, StudentId = 2, CourseId = 55, Grade = 60 },
                new Enrollment() { EnrollmentId = 5, StudentId = 4, CourseId = 55, Grade = 80 },
                new Enrollment() { EnrollmentId = 6, StudentId = 6, CourseId = 55, Grade = 100 },
                new Enrollment() { EnrollmentId = 7, StudentId = 8, CourseId = 55, Grade = 100 },
                new Enrollment() { EnrollmentId = 9, StudentId = 10, CourseId = 55, Grade = 150 },
                new Enrollment() { EnrollmentId = 10, StudentId = 12, CourseId = 55, Grade = 200 },

                new Enrollment() { EnrollmentId = 11, StudentId = 2, CourseId = 60, Grade = 400 },
                new Enrollment() { EnrollmentId = 12, StudentId = 4, CourseId = 60, Grade = 200 },
            });

            await context.SaveChangesAsync();

            // Act
            var result = new GetTopGradesStudentsByCourseQuery.GetTopGradesStudentsByCourseQueryHandler(context).Handle(args, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsTrue(result.Result.TopGradeCourses.Count() == 3);

            var jsCourseStudents = result.Result.TopGradeCourses.Where(x => x.Course == "JS 2022").SelectMany(x => x.Students).Select(x => x).ToList();
            Assert.IsTrue(jsCourseStudents.Count() == 3);
            Assert.IsTrue(jsCourseStudents.Any(x => x.FirstName == "Aubrie" && x.LastName == "McCaw"));
            Assert.IsTrue(jsCourseStudents.Any(x => x.FirstName == "Findley" && x.LastName == "Anster"));
            Assert.IsTrue(jsCourseStudents.Any(x => x.FirstName == "Jedidiah" && x.LastName == "Stegers"));

            var netCourseStudents = result.Result.TopGradeCourses.Where(x => x.Course == ".NET 2022").SelectMany(x => x.Students).Select(x => x).ToList();
            Assert.IsTrue(netCourseStudents.Count() == 4);
            Assert.IsTrue(netCourseStudents.Any(x => x.FirstName == "Jammal" && x.LastName == "Darkin"));
            Assert.IsTrue(netCourseStudents.Any(x => x.FirstName == "Talyah" && x.LastName == "Papes"));
            Assert.IsTrue(netCourseStudents.Any(x => x.FirstName == "Jedidiah" && x.LastName == "Stegers"));
            Assert.IsTrue(netCourseStudents.Any(x => x.FirstName == "Abbot" && x.LastName == "Le Strange"));

            var cssCourseStudents = result.Result.TopGradeCourses.Where(x => x.Course == "CSS 2022").SelectMany(x => x.Students).Select(x => x).ToList();
            Assert.IsTrue(cssCourseStudents.Count() == 2);
            Assert.IsTrue(cssCourseStudents.Any(x => x.FirstName == "Aubrie" && x.LastName == "McCaw"));
            Assert.IsTrue(cssCourseStudents.Any(x => x.FirstName == "Findley" && x.LastName == "Anster"));
        }
    }
}
