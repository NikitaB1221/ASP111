using ASP111.Data;
using ASP111.Models.Forum.Index;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP111.Controllers
{
    [Route("api/section")]
    [ApiController]
    public class BackSectionController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public BackSectionController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public IEnumerable<ForumSectionViewModel> GetAll()
        {
            int n = 0;
            return _dataContext
        .Sections
        .Include(s => s.Author)
        .Include(s => s.Rates)
        .Where(s => s.DeleteDt == null)
        .OrderBy(s => s.CreateDt)
        .AsEnumerable()
        .Select(s => new ForumSectionViewModel
        {
            Id = s.Id.ToString(),
            Title = s.Title,
            Description = s.Description,
            CreateDt = s.CreateDt.ToShortDateString(),
            ImageUrl = s.ImageUrl == null
            ? $"/img/S/section{n++}.png"
            : $"/img/{s.ImageUrl }",
            Author = new(s.Author),



            Likes = s.Rates.Count(r => r.Rating > 0),
            Dislikes = s.Rates.Count(r => r.Rating < 0)
        });
        }

    }
}

    /*CRUD-полнота - свойство контроллера, означающее что в нем 
     * реализованы методы CRUD
     */

