using CompleteExample.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CompleteExample.Logic.Queries
{
    public class EnrollStudentCourseCommand
    {
        public class Arguments : IRequest<int>
        {
            public int StudentId { get; set; }
            public int CourseId { get; set; }
        }

        public class EnrollStudentCourseCommandHandler : IRequestHandler<Arguments, int>
        {
            private readonly ICompleteExampleDBContext _context;
            public EnrollStudentCourseCommandHandler(ICompleteExampleDBContext context)
            {
                this._context = context;
            }
            public async Task<int> Handle(Arguments arguments, CancellationToken cancellationToken)
            {
                var enrollment = new Enrollment() { CourseId = arguments.CourseId, StudentId = arguments.StudentId };

                //According to the scope of the challenge, I kept it simple but thinking of escalating it out(and I really don't know if
                //the exercise allowed creating new layers) I would create a CompleteExample.Infrastructure layer where I would put Readers and Writers,
                //moving the data access logic.
                this._context.Enrollment.Add(enrollment);

                await this._context.SaveChanges();

                return enrollment.EnrollmentId;
            }
        }
    }
}
