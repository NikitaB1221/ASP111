using ASP111.Data;
using ASP111.Data.Entities;
using ASP111.Models;
using ASP111.Models.Forum.Index;
using ASP111.Models.Forum.Section;
using ASP111.Models.Forum.Theme;
using ASP111.Models.Forum.Topic;
using ASP111.Services.AuthUser;
using ASP111.Services.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace ASP111.Controllers
{
    public class ForumController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<ForumController> _logger;
        private readonly IAuthUserService _authUserService;
        private readonly IValidationService _validationService;

        public ForumController(DataContext dataContext, ILogger<ForumController> logger, IAuthUserService authUserService, IValidationService validationService)
        {
            _dataContext = dataContext;
            _logger = logger;
            _authUserService = authUserService;
            _validationService = validationService;
        }

        //                     <a  asp-route-id="@..." 
        public IActionResult Theme([FromRoute] Guid id)
        {
            var theme = _dataContext
                .Themes
                .Include(t => t.Author)
                .Where(t => t.Id == id && t.DeleteDt == null)
                .FirstOrDefault();



            if (theme == null)
            {
                return NotFound();
            }
            ThemePageModel model = new()
            {
                Theme = new(theme),
                Comments = _dataContext.Comments.Include(c => c.Author).OrderBy(c => c.CreatedDt).Where(c => c.DeleteDt == null && c.ThemeId == id).Select(c => new CommentViewModel(c)).ToList()
            };
            Guid? AuthUserId = _authUserService.GetUserId(HttpContext);
            if (AuthUserId != null)
            {
                ViewData["authUser"] = new UserViewModel(_dataContext.Users.Find(AuthUserId.Value)!);
            }
            return View(model);
        }

        [HttpPost]
        public RedirectToActionResult AddComment([FromForm]CommentFormModel formModel)
        {
            var messages = _validationService.ErrorMessages(formModel);
            foreach (var (key, message) in messages)
            {
                if (message != null)  // есть сообщение об ошибке
                {
                    HttpContext.Session.SetString(
                        "AddTopicMessage",
                        JsonSerializer.Serialize(messages)
                    );
                    return RedirectToAction(nameof(Theme), new { id = formModel.ThemeId });
                }

                Guid? userId = _authUserService.GetUserId(HttpContext);
                if (userId != null)
                {
                    DateTime DT = DateTime.Now;
                    _dataContext.Comments.Add(new()
                    {
                        Id = Guid.NewGuid(),
                        AuthorId = userId.Value,
                        Content = formModel.Content,
                        ThemeId = formModel.ThemeId,
                        CreatedDt = DT,
                        ReplyId = formModel.ReplyId,
                    });
                    _dataContext.SaveChanges();
                }
            }
            return RedirectToAction(nameof(Theme), new { id = formModel.ThemeId });
        }


        public IActionResult Topic([FromRoute] Guid id)
        {
            var topic = _dataContext
                .Topics
                .Include(t => t.Author)
                .FirstOrDefault(t => t.Id == id);

 

            if (topic == null)
            {
                return NotFound();
            }

 

            TopicPageModel model = new()
            {
                Topic = new(topic)
            };
            model.Themes = _dataContext
                .Themes
                .Include(t => t.Author)
                .Include(t => t.Comments)
                .Where(t => t.TopicId == topic.Id && t.DeleteDt == null)
                .Select(t => new ThemeViewModel(t))
                .ToList();

 

            if (HttpContext.Session.Keys.Contains("AddThemeMessage"))
            {
                model.ErrorMessages =
                    JsonSerializer.Deserialize<Dictionary<String, String?>>(
                        HttpContext.Session.GetString("AddThemeMessage")!);

 

                HttpContext.Session.Remove("AddThemeMessage");
            }

 

            return View(model);
        }

        public IActionResult Section([FromRoute] Guid id)
        {
            var section = _dataContext.Sections
                .Include(s => s.Author)
                .FirstOrDefault(s => s.Id == id);

            if (section == null)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return NotFound();
            }
            SectionPageModel sectionViewModel = new()
            {
                Section = new ForumSectionViewModel
                {
                    Id = section.Id.ToString(),
                    Title = section.Title,
                    Description = section.Description,
                    CreateDt = section.CreateDt.ToShortDateString(),
                    Author = new(section.Author),
                }
            };
            if (HttpContext.Session.Keys.Contains("AddTopicMessage"))
            {
                sectionViewModel.ErrorMessages =
                    JsonSerializer.Deserialize<Dictionary<String, String?>>(
                        HttpContext.Session.GetString("AddTopicMessage")!);

                HttpContext.Session.Remove("AddTopicMessage");
            }
            sectionViewModel.Topics =
            _dataContext.Topics.Include(t => t.Author).Where(t => t.DeleteDt == null).OrderByDescending(t => t.CreatedDt).AsEnumerable().Select(t => new TopicViewModel()
            {
                Id = t.Id.ToString(),
                Title = t.Title,
                Description = t.Description,
                CreatedDt = t.CreatedDt.ToShortDateString(),
                ImageUrl = "/img/" + t.ImageUrl,
                Author = new(t.Author),
            }).ToList();

            return View(sectionViewModel);
        }

        [HttpPost]
        public RedirectToActionResult AddTopic(TopicFormModel formModel)
        {
            var messages = _validationService.ErrorMessages(formModel);
            foreach (var (key, message) in messages)
            {
                if (message != null)  // есть сообщение об ошибке
                {
                    HttpContext.Session.SetString(
                        "AddTopicMessage",
                        JsonSerializer.Serialize(messages)
                    );
                    return RedirectToAction(nameof(Section), new { id = formModel.SectionId });
                }
            }
            // проверяем что пользователь аутентифицирован
            Guid? userId = _authUserService.GetUserId(HttpContext);
            if (userId != null)
            {
                String? nameAvatar = null;
                if (formModel.ImageFile != null)
                {
                    // определяем расширение файла
                    String ext = Path.GetExtension(formModel.ImageFile.FileName);
                    // проверить расширение на перечень допустимых

                    // формируем имя для файла
                    nameAvatar = Guid.NewGuid().ToString() + ext;

                    using FileStream fstream = new(
                        "wwwroot/img/" + nameAvatar,
                        FileMode.Create);

                    formModel.ImageFile.CopyTo(fstream);
                }

                _dataContext.Topics.Add(new()
                {
                    Id = Guid.NewGuid(),
                    AuthorId = userId.Value,
                    SectionId = formModel.SectionId,
                    Title = formModel.Title,
                    Description = formModel.Description,
                    CreatedDt = DateTime.Now,
                    ImageUrl = nameAvatar
                });
                _dataContext.SaveChanges();
                _logger.LogInformation("Add Section OK");
            }

            return RedirectToAction(nameof(Section), new { id = formModel.SectionId });
        }

        public IActionResult Index()
        {
            int n = 0;
            ForumIndexModel model = new()
            {
                Sections = _dataContext
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
                            : $"/img/S/{s.ImageUrl}",
                        Author = new(s.Author),
                        Likes = s.Rates.Count(r => r.Rating > 0),
                        Dislikes = s.Rates.Count(r => r.Rating < 0),

                    }),

                // проверяем есть ли в сессии сообщение о валидации формы,
                // если есть, извлекаем, десериализуем и передаем на 
                // представление (все сообщения) вместе с данными формы, которые
                // подставятся обратно в поля формы
            };


            return View(model);
            // В представлении проверяем наличие данных валидации
            // если они есть, то в целом форма не принята,
            // выводим сообщения под каждым полем:
            // если сообщение null, то нет ошибок, поле принято
            // иначе - ошибка и ее текст в сообщении
        }

        [HttpPost]
        public RedirectToActionResult AddSection(ForumSectionFormModel model)
        {
            var messages = _validationService.ErrorMessages(model);
            foreach (var (key, message) in messages)
            {
                if (message != null)
                {
                    HttpContext.Session.SetString(
                        "AddSectionMessage",
                        JsonSerializer.Serialize(messages)
                    );
                    return RedirectToAction(nameof(Section));
                }
            }
            // проверяем что пользователь аутентифицирован
            Guid? userId = _authUserService.GetUserId(HttpContext);
            if (userId != null)
            {
                _dataContext.Sections.Add(new()
                {
                    Id = Guid.NewGuid(),
                    Title = model.Title,
                    Description = model.Description,
                    CreateDt = DateTime.Now,
                    ImageUrl = null,
                    DeleteDt = null,
                    AuthorId = userId.Value,
                });
                _dataContext.SaveChanges();
                _logger.LogInformation("Add Section OK");
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public RedirectToActionResult AddTheme(ThemeFormModel formModel)
        {
            var messages = _validationService.ErrorMessages(formModel);
            foreach (var (key, message) in messages)
            {
                if (message != null)  // есть сообщение об ошибке
                {
                    HttpContext.Session.SetString(
                        "AddThemeMessage",
                        JsonSerializer.Serialize(messages)
                    );
                    return RedirectToAction(nameof(Topic), new { id = formModel.TopicId });
                }
            }
            // проверяем что пользователь аутентифицирован
            Guid? userId = _authUserService.GetUserId(HttpContext);
            if (userId != null)
            {
                Guid themeId = Guid.NewGuid();
                DateTime DT = DateTime.Now;
                _dataContext.Themes.Add(new()
                {
                    Id = themeId,
                    AuthorId = userId.Value,
                    TopicId = formModel.TopicId,
                    Title = formModel.Title,
                    CreatedDt = DT,
                });
                _dataContext.Comments.Add(new()
                {
                    Id = Guid.NewGuid(),
                    AuthorId = userId.Value,
                    Content = formModel.Content,
                    ThemeId = themeId,
                    CreatedDt= DT,
                });
                _dataContext.SaveChanges();
            }
            return RedirectToAction(nameof(Topic), new { id = formModel.TopicId });
        }
    }
}
/* Задача: валидация (сервис валидации)
 * Задание: реализовать средства проверки моделей форм на правильность данных
 * Особенности: разные поля нужно проверять по-разному, а в разных моделях
 *  бывают одинаковые правила проверки.
 * + Нужно формирование сообщений о результатах проверки 
 * Готовые решения:
 *  https://learn.microsoft.com/en-us/aspnet/core/mvc/models/validation
 *  
 * Идея:
 * class Model {
 *  ...
 *  [ValidationRules(ValidationRule.NotEmpty, ValidationRule.Name)]
 *  String name
 *  
 *  [ValidationRules(ValidationRule.NotEmpty, ValidationRule.Password)]
 *  String password
 *  
 *  [ValidationRules(ValidationRule.Login)]
 *  String login
 *  
 *  }
 *  
 *  _service.ErrorMessages(model) 
 *    [ "name" => "Не может быть пустым", 
 *      "password" => "должен содержать цифру",
 *      "login" => null ]
 */
/* o LINQ
 * LINQ-to-SQL (LINQ-to-Entity)
 *  - собирает информацию о запросе и строит его SQL выражение
 *  _context.Users.Where(u => u.Name == 'Admin' )
 *  ==> IQueryable ( "SELECT * FROM Users u WHERE u.Name = 'Admin'
 *  
 * LINQ-to-Objects
 *  - циклическая (итеративная) обработка коллекций
 *  collection.Users.Where(u => u.Name == 'Admin' )
 *  ==> IEnumerable
 *  
 *  _context.Users.Where(u => u.Name == 'Admin' ).ToList()
 *  _context.Users.ToList().Where(u => u.Name == 'Admin' )
 */