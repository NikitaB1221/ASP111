using Microsoft.AspNetCore.Mvc;

namespace ASP111.Controllers
{
    public class FrontController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
