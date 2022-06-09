using Microsoft.AspNetCore.Mvc;

namespace Journeyman.Persons.Micro.Controllers
{
    [ApiController, Route("api/person/[controller]")]
    public class TestController : ControllerBase
    {
        public ActionResult<string> Get()
        {
            return Ok("Hello world.");
        }
    }
}
