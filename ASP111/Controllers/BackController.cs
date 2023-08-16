using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASP111.Controllers
{
    [Route("api/back")]
    [ApiController]
    public class BackController : ControllerBase
    {
        [HttpGet]
        public object Get( int x, int y) 
        {
            QueryString QueryString = Request.QueryString;
            if (!Request.Query.ContainsKey("y"))
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return new { Error = true, Message = "Parameter 'y' is required" };
            }
            return new { 
                message = "Hello from GET method",
                x,
                y,
                my = Request.Headers["My-Header"],
            };
        }

        [HttpPost]
        public object DoPost() 
        {
            return new { message = "Hello from POST method" };
        }

        [HttpPut]
        public object PutThis() 
        {
            return new { message = "Hello from PUT method" };
        }

        [HttpDelete]
        public object LetsDelete() 
        {
            return new { message = "Succesfuly Deleted!" };
        }
    }
}
