using CompleteExample.Logic.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CompleteExample.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public InstructorsController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        /// <summary>
        /// Gets Students Grades by InstructorId.
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}/students-grades")]
        public async Task<IActionResult> GetStudentsGrades(int id)
        {
            return Ok(await this._mediator.Send(new GetStudentsGradesByInstructorQuery.Arguments { InstructorId = id }));
        }
    }
}
