using CompleteExample.Entities;
using CompleteExample.Logic.Queries;
using FluentValidation;
using System.Linq;

namespace CompleteExample.Logic.Validators
{
    public class UpdateStudentGradeCommandArgumentsValidator : AbstractValidator<UpdateStudentGradeCommand.Arguments>
    {
        public UpdateStudentGradeCommandArgumentsValidator(ICompleteExampleDBContext context)
        {
            RuleFor(x => x.CourseId)
            .Must(id =>
            {
                return context.Courses.Any(x => x.CourseId == id);
            })
            .WithMessage("Course must exist.");

            RuleFor(x => x.StudentId)
                .Must(id =>
                {
                    return context.Students.Any(x => x.StudentId == id);
                })
            .WithMessage("Student must exist.");

            RuleFor(x => x.Grade).ScalePrecision(2, 5);

            RuleFor(x => x.Grade).GreaterThanOrEqualTo(0);

            RuleFor(x => x.Grade).LessThan(1000);
        }
    }
}
