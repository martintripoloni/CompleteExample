using CompleteExample.Logic.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CompleteExample.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public EnrollmentsController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        /// <summary>
        /// Enroll a student in a course
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Enroll(EnrollStudentCourseCommand.Arguments arguments)
        {
            return Ok(await this._mediator.Send(arguments));
        }

        /// <summary>
        /// Update a grade(number) for a student for a course
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> UpdateGrade(UpdateStudentGradeCommand.Arguments arguments)
        {
            return Ok(await this._mediator.Send(arguments));
        }
    }
}
