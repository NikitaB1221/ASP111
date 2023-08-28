using ASP111.Data;
using ASP111.Models.Forum.Section;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP111.Controllers
{
    [Route("api/topic")]
    [ApiController]
    public class BackTopicController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public BackTopicController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public IEnumerable<TopicViewModel>? GetTopics(Guid SectionId) 
        {
            var section = _dataContext.Sections
                .Include(s => s.Author)
                .FirstOrDefault(s => s.Id == SectionId);
 
            if ( section == null )
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return null;
            }

            return _dataContext.Topics
                .Include(t => t.Author)
                .Where(t => t.DeleteDt == null)
                .OrderByDescending(t => t.CreatedDt)
                .AsEnumerable()
                .Select(t => new TopicViewModel()
                {
                    Id = t.Id.ToString(),
                    Title = t.Title,
                    Description = t.Description,
                    CreatedDt = t.CreatedDt.ToShortDateString(),
                    ImageUrl = "/img/" + t.ImageUrl,
                    Author = new(t.Author)
                });
        }
    }
}
