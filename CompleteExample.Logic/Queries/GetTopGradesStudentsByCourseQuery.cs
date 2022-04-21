using CompleteExample.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CompleteExample.Logic.Queries
{
    public class GetTopGradesStudentsByCourseQuery
    {
        public class Arguments : IRequest<Result>
        {
        }

        public class Result
        {
            public IEnumerable<TopGradeCourse> TopGradeCourses { get; set; }

            public class TopGradeCourse
            {
                public string Course { get; set; }
                public IEnumerable<TopGradeCourseStudent> Students { get; set; }
                public class TopGradeCourseStudent
                {
                    public string FirstName { get; set; }
                    public string LastName { get; set; }
                }
            }
        }

        public class GetTopGradesStudentsByCourseQueryHandler : IRequestHandler<Arguments, Result>
        {
            private readonly ICompleteExampleDBContext _context;
            public GetTopGradesStudentsByCourseQueryHandler(ICompleteExampleDBContext context)
            {
                this._context = context;
            }
            public async Task<Result> Handle(Arguments queryArguments, CancellationToken cancellationToken)
            {
                var topGradeCourses = new List<Result.TopGradeCourse>();

                //According to the scope of the challenge, I kept it simple but thinking of escalating it out(and I really don't know if
                //the exercise allowed creating new layers) I would create a CompleteExample.Infrastructure layer where I would put Readers and Writers,
                //moving the data access logic.
                var coruseNotes = this._context.Courses
                                .Include(x => x.Enrollments)
                                .Include("Enrollments.Student")
                                .Select(x => new
                                 {
                                     Course = x,
                                     Grades = x.Enrollments.Where(y => y.Grade.HasValue).Select(y => y.Grade).ToList()
                                 }).ToList();

                foreach (var courseNotes in coruseNotes)
                {
                    var notes = courseNotes.Grades.Distinct().OrderByDescending(x => x).ToList();

                    if (notes.Count() > 3)
                    {
                        notes = notes.Take(3).ToList();
                    }

                    var students = courseNotes.Course.Enrollments.Where(x => x.Grade.HasValue).Where(x => notes.Contains(x.Grade.Value))
                        .Select(e => new Result.TopGradeCourse.TopGradeCourseStudent
                        {
                            FirstName = e.Student.FirstName,
                            LastName = e.Student.LastName,
                        }).ToList();

                    topGradeCourses.Add(new Result.TopGradeCourse
                    {
                        Course = courseNotes.Course.Title,
                        Students = students,
                    });
                }

                return new Result { TopGradeCourses = topGradeCourses };
            }
        }
    }
}
