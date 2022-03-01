using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WebApiControllerBase : ControllerBase
    {
        protected ObjectResult Forbidden(string message) => StatusCode((int)HttpStatusCode.Forbidden, message);
        protected ObjectResult InternalServerError(string message) => StatusCode((int)HttpStatusCode.InternalServerError, message);
    }
}
