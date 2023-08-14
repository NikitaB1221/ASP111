using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASP111.Controllers
{
    [Route("api/back")]
    [ApiController]
    public class BackController : ControllerBase
    {
        [HttpGet]
        public object Get() 
        {
            return new { message = "Hello from GET method" };
        }

        [HttpPost]
        public object DoPost() 
        {
            return new { message = "Hello from POST method" };
        }
    }
}
