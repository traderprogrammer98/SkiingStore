using Microsoft.AspNetCore.Mvc;

namespace SkiingStore.Controllers
{
    public class BuggyController : BaseApiController
    {
        [HttpGet("not-found")]
        public IActionResult GetNotFound()
        {
            return NotFound();
        }
        [HttpGet("bad-request")]
        public IActionResult GetBadRequest()
        {
            return BadRequest(new ProblemDetails { Title = "this is a bad request." });
        }
        [HttpGet("unauthorized")]
        public IActionResult GetUnauthorized()
        {
            return Unauthorized();
        }
        [HttpGet("validation-error")]
        public IActionResult GetValidationError()
        {
            ModelState.AddModelError("problem1", "this is first problem.");
            ModelState.AddModelError("problem2", "this is second problem.");
            ModelState.AddModelError("problem3", "this is third problem.");

            return ValidationProblem();
        }
        [HttpGet("server-error")]
        public IActionResult GetServerError()
        {
            throw new Exception("this is a server error");
        }
      
    }
}
