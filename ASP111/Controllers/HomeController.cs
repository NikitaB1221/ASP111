using ASP111.Models;
using ASP111.Services;
using ASP111.Services.Hash;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ASP111.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        // декларируем зависимость
        private readonly IDateService _dateService;
        private readonly TimeService _timeService;
        private readonly DateTimeService _dateTimeService;
        private readonly ValidatorService _validatorService;

        // признак readonly говорит о том, что данные должны инициализироваться конструктором
        public HomeController(                // параметр в конструкторе требует передачи ссылки,
            ILogger<HomeController> logger,   // иначе объект контроллера не может быть построен
            IDateService dateService,          // - это является зависимостью
            TimeService timeService,
            DateTimeService dateTimeService,
            ValidatorService validatorService)
        {
            _logger = logger;
            _dateService = dateService;
            _timeService = timeService;
            _dateTimeService = dateTimeService;
            _validatorService = validatorService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Intro()
        {
            return View();
        }

        public IActionResult Razor()
        {
            return View();
        }

        public ViewResult Services()
        {
            ViewData["date"] = _dateService.GetDate();
            ViewData["time"] = _timeService.GetTime();
            ViewData["datetime"] = _dateTimeService.GetNow();

            ViewData["datetime-hash"] = _dateTimeService.GetHashCode();
            ViewData["time-hash"] = _timeService.GetHashCode();
            ViewData["date-hash"] = _dateService.GetHashCode();
            ViewData["ctrl-hash"] = this.GetHashCode();

            ViewData["res"] = new bool[] {    
                _validatorService.ValidateLogin("NormalLogin1"),
                _validatorService.ValidateLogin("Normal_Login_2"),
                _validatorService.ValidateLogin("In-valid login"),
                _validatorService.ValidateLogin("$ invalid ++")
            };

            return View();
        }

        public ViewResult Data()
        {
            return View();
        }

        public ViewResult Sessions()
        {
            if (HttpContext.Session.Keys.Contains("StoredValue"))
            {
                ViewData["StoredValue"] = HttpContext.Session.GetString("StoredValue");
            }
            else
            {
                ViewData["StoredValue"] = "";
            }
            return View();
        }

        public RedirectToActionResult SetSession()
        {
            HttpContext.Session.SetString("StoredValue", DateTime.Now.ToString());
            return RedirectToAction(nameof(Sessions));
        }

        public RedirectToActionResult RemoveSession() 
        {
            HttpContext.Session.Remove("StoredValue");
            return RedirectToAction(nameof(Sessions));
        }

        public ViewResult Middleware()
        {
            ViewData["marker"] = HttpContext.Items.ContainsKey("marker") ? HttpContext.Items["marker"] : "Not found!";
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}