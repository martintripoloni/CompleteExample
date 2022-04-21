using CompleteExample.Entities;
using CompleteExample.Logic.Queries;
using FluentValidation;
using System.Linq;

namespace CompleteExample.Logic.Validators
{
    public class EnrollStudentCourseCommandArgumentsValidator : AbstractValidator<EnrollStudentCourseCommand.Arguments>
    {
        public EnrollStudentCourseCommandArgumentsValidator(ICompleteExampleDBContext context)
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

            RuleFor(x => x)
               .Must(enrollment =>
               {
                   return !context.Enrollment.Any(x => x.CourseId == enrollment.CourseId && x.StudentId == enrollment.StudentId);
               })
           .WithMessage("The student is already registered for that course.");
        }
    }
}
