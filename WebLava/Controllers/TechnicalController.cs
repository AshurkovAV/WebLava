using Microsoft.AspNetCore.Mvc;

namespace WebLava.Controllers
{
    public class TechnicalController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
