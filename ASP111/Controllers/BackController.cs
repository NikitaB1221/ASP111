using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASP111.Controllers
{
    [Route("api/back")]
    [ApiController]
    public class BackController : ControllerBase
    {
        [HttpGet]
        public object Get(int x, int y)
        {
            QueryString QueryString = Request.QueryString;
            if (!Request.Query.ContainsKey("y"))
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return new { Error = true, Message = "Parameter 'y' is required" };
            }
            return new
            {
                message = "Hello from GET method",
                x,
                y,
                my = Request.Headers["My-Header"],
            };
        }

        [HttpPost]
        public object DoPost(PostBody postBody)  //  [FromBody] dynamic postBody
        {
            return new { message = "Hello from POST method", postBody };
        }

        [HttpPut]
        public object PutThis(PutBody putBody, String nd)
        {
            QueryString QueryString = Request.QueryString;
            if (!Request.Query.ContainsKey("nd"))
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return new { Error = true, Message = "Parameter 'nd' is required" };
            }
            putBody.Data += $" {nd}";
            return new { message = $"\"{nd}\" was PUTted", putBody};
        }

        [HttpDelete]
        public object LetsDelete(String data)
        {
            QueryString QueryString = Request.QueryString;
            if (!Request.Query.ContainsKey("data"))
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return new { Error = true, Message = "Parameter 'data' is required" };
            }

            return new { target = data, message = "Succesfuly Deleted!"  };

        }
    }

    public class PostBody  //  ORM  ==  C# object for containig JSON object
    {
        public String Data { get; set; }  //  property name == JSON obj name

    }

    public class PutBody
    {
        public String Data { get; set; }  //  property name == JSON obj name

    }
}
