using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.Common.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WebApiControllerBase : ControllerBase
    {
        public ObjectResult Forbidden(string message) => StatusCode((int)HttpStatusCode.Forbidden, message);
        public ObjectResult InternalServerError(string message) => StatusCode((int)HttpStatusCode.InternalServerError, message);
    }
}
