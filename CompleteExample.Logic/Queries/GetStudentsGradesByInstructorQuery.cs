using CompleteExample.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CompleteExample.Logic.Queries
{
    public class GetStudentsGradesByInstructorQuery
    {
        public class Arguments : IRequest<Result>
        {
            public int InstructorId { get; set; }
        }

        public class Result
        {
            public IEnumerable<StudentsGrade> StudentsGrades { get; set; }

            public class StudentsGrade
            {
                public string Course { get; set; }
                public string FirstName { get; set; }
                public string LastName { get; set; }
                public IEnumerable<Decimal> Grades { get; set; }
            }
        }

        public class GetStudentsGradesByInstructorQueryHandler : IRequestHandler<Arguments, Result>
        {
            private readonly ICompleteExampleDBContext _context;
            public GetStudentsGradesByInstructorQueryHandler(ICompleteExampleDBContext context)
            {
                _context = context;
            }
            public async Task<Result> Handle(Arguments queryArguments, CancellationToken cancellationToken)
            {
                //According to the scope of the challenge, I kept it simple but thinking of escalating it out(and I really don't know if
                //the exercise allowed creating new layers) I would create a CompleteExample.Infrastructure layer where I would put Readers and Writers,
                //moving the data access logic.
                var studentsGrades = this._context.Instructors
                                     .Where(i => i.InstructorId == queryArguments.InstructorId)
                                     .SelectMany(i => i.Courses)
                                     .SelectMany(c => c.Enrollments)
                                     .Where(e => e.Grade.HasValue)
                                     .Select(e =>
                                             new Result.StudentsGrade
                                             {
                                                 Course = e.Course.Title,
                                                 FirstName = e.Student.FirstName,
                                                 LastName = e.Student.LastName,
                                                 Grades = new List<decimal>() { e.Grade.Value }
                                                          .Union(e.EnrollmentHistory.Where(y => y.Grade.HasValue).OrderByDescending(x => x.Id).Select(h => h.Grade.Value).ToList()),
                                             })
                                     .ToList();

                return new Result { StudentsGrades = studentsGrades };
            }
        }
    }
}
