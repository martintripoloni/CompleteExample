using CompleteExample.Entities;
using CompleteExample.Logic.Queries;
using FluentValidation;
using System.Linq;

namespace CompleteExample.Logic.Validators
{
    public class GetStudentsGradesByInstructorQueryArgumentsValidator : AbstractValidator<GetStudentsGradesByInstructorQuery.Arguments>
    {
        public GetStudentsGradesByInstructorQueryArgumentsValidator(ICompleteExampleDBContext context)
        {
            RuleFor(x => x.InstructorId)
            .Must(id =>
            {
                return context.Instructors.Any(x => x.InstructorId == id);
            })
            .WithMessage("Instructor must exist.");
        }
    }
}
