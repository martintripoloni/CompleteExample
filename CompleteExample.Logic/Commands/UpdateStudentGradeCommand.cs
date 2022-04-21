using CompleteExample.Entities;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CompleteExample.Logic.Queries
{
    public class UpdateStudentGradeCommand
    {
        public class Arguments : IRequest<int?>
        {
            public int StudentId { get; set; }
            public int CourseId { get; set; }
            public decimal Grade { get; set; }
        }

        public class UpdateStudentGradeCommandHandler : IRequestHandler<Arguments, int?>
        {
            private readonly ICompleteExampleDBContext _context;
            public UpdateStudentGradeCommandHandler(ICompleteExampleDBContext context)
            {
                this._context = context;
            }
            public async Task<int?> Handle(Arguments arguments, CancellationToken cancellationToken)
            {
                //According to the scope of the challenge, I kept it simple but thinking of escalating it out(and I really don't know if
                //the exercise allowed creating new layers) I would create a CompleteExample.Infrastructure layer where I would put Readers and Writers,
                //moving the data access logic.
                var enrollment = this._context.Enrollment
                                   .Where(i => i.CourseId == arguments.CourseId &&
                                               i.StudentId == arguments.StudentId)
                                   .First();
                
                enrollment.Grade = arguments.Grade;

                await this._context.SaveChanges();

                return enrollment.EnrollmentId;
            }
        }
    }
}
