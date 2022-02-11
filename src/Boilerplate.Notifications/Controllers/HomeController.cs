using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.Notifications.Controllers
{
    [Route("")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("Boilerplate.Notifications Service");
    }
}
