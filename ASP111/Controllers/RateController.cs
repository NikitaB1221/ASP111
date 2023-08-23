using ASP111.Data;
using ASP111.Services.AuthUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace ASP111.Controllers
{
    [Route("api/rate")]
    [ApiController]
    public class RateController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IAuthUserService _authService;


        public RateController(DataContext dataContext, IAuthUserService authService)
        {
            _dataContext = dataContext;
            _authService = authService;
        }

        [HttpGet]
        public object DoGet()
        {
            return new { message = "You've GET this!" };
        }

        [HttpPost]
        public object DoPost([FromQuery]Guid itemId, [FromQuery]int rateValue)
        {
            Guid? userId = _authService.GetUserId(HttpContext);
            if (userId is null)
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                return "Authorization required!";
            }
            if (itemId == default(Guid))
            {
                Response.StatusCode = StatusCodes.Status406NotAcceptable;
                return "Invalid parameter: itemId";
            }
            if(rateValue is 0)
            {
                Response.StatusCode = StatusCodes.Status406NotAcceptable;
                return "Invalid parameter: rateValue";
            }
            var rate = new Data.Entities.Rate() 
            {
                ItemId = itemId,
                UserId = userId.Value,
                Moment = DateTime.Now,
                Rating = rateValue
            };
            _dataContext.Rates.Add(rate);
            try
            {
                _dataContext.SaveChanges();
            }
            catch (Exception)
            {
                Response.StatusCode = StatusCodes.Status406NotAcceptable;
                return "Invalid parameter: already exist";
            }

            Response.StatusCode = StatusCodes.Status201Created;
            return new{ rateNum = _dataContext.Rates.Where(r => r.ItemId == itemId).Count(r => r.Rating == rateValue)};
        }

        [HttpPut]
        public object DoPut([FromBody] dynamic body)
        {
            return body;
        }

        [HttpDelete]
        public object DoDelete([FromQuery] String id)
        {
            return new { id };
        }

        private object DoDefault()
        {
            return this.Request.Method switch
            {
                "LINK" => DoLink(),
                "UNLINK" => DoUnlink(),
                "HELLO" => DoHello(),
                _ => new { method = Request.Method }
            };
        }

        private object DoLink()
        {
            return new { link = true, id = Guid.NewGuid() };
        }
        private object DoUnlink()
        {
            return new { link = false, id = Guid.Empty };
        }
        private IEnumerable<String> DoHello()
        {
            return new String[]
            {
                "Hello",
                "World",
                "!"
            };
        }
        //private 
    }
}
