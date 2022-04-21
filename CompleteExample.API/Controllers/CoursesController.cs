using CompleteExample.Logic.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CompleteExample.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CoursesController(IMediator mediator)
        {
            this._mediator = mediator;
        }
        /// <summary>
        /// Gets List all students that have the top 3 grades for each course.
        /// </summary>
        /// <returns></returns>
        [HttpGet("top-grades")]
        public async Task<IActionResult> TopGrades()
        {
            return Ok(await this._mediator.Send(new GetTopGradesStudentsByCourseQuery.Arguments {  }));
        }
    }
}
