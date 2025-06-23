using Microsoft.AspNetCore.Mvc;

namespace WebLava.Controllers
{
    public class QuamDevelopMobiliController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Blog/QuamDevelopMobili/Index.cshtml");
        }
    }
}
