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

        [HttpPost]
        public object AddSection([FromBody]SectionData sectionData)
        {
            if (sectionData == null)
            {
                Response.StatusCode = StatusCodes.Status406NotAcceptable;
                return new { Status = "406", Message = "SectionData required" };
            }
            if (String.IsNullOrEmpty(sectionData.Title) || String.IsNullOrEmpty(sectionData.Description))
            {
                Response.StatusCode = StatusCodes.Status406NotAcceptable;
                return new { Status = "406", Message = "SectionData required" };
            }
            if (_dataContext.Sections.Any(s => s.Title == sectionData.Title))
            {
                Response.StatusCode = StatusCodes.Status409Conflict;
                return new { Status = "409", Message = "Section with this title already exist, sectionData conflicts" };
            }

            Guid id = Guid.NewGuid();
            _dataContext.Sections.Add(new()
            {
                Id = id,
                Title = sectionData.Title,
                Description = sectionData.Description,
                CreateDt = DateTime.Now,
                AuthorId = _dataContext.Users.First().Id,
                DeleteDt = null
            }); 
            _dataContext.SaveChanges();
            return new { StatusCode = "200", Message = id.ToString() };
        }
    }
    public class SectionData
    {
        public String Title { get; set; } = null!;
        public String Description { get; set; } = null!;
    }
}

    /*CRUD-полнота - свойство контроллера, означающее что в нем 
     * реализованы методы CRUD
     * 
     * C - Create, обычно реализуется методом POST
     * Существует два подхода 
     * - используються запросы типа multipart
     * - использование для основных запросов JSON, а файлы, 
     *   при необходимости, отправлять запросом PATCH
     * = Или принципиально другой подхлд - файлами занимается отдельная служба
     *   (микросервис), а в Бэк передается имя (ссылка) файла
     */

