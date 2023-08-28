using ASP111.Data;
using ASP111.Models.Forum.Section;
using ASP111.Models.Forum.Theme;
using ASP111.Models.Forum.Topic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP111.Controllers
{
    [Route("api/theme")]
    [ApiController]
    public class BackThemeController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public BackThemeController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public IEnumerable<ThemeViewModel>? GetThemes(Guid topicId) 
        {
            var topic = _dataContext.Topics
                .Include(s => s.Author)
                .FirstOrDefault(s => s.Id == topicId);

            if (topic == null)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return null;
            }

            return (IEnumerable<ThemeViewModel>?)_dataContext.Themes
                .Include(t => t.Author)
                .Where(t => t.DeleteDt == null)
                .OrderByDescending(t => t.CreatedDt)
                .AsEnumerable()
                .Select(t => new ThemeViewModel(t));
        }

    }
}
