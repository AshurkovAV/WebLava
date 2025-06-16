using Microsoft.AspNetCore.Mvc;

namespace WebLava.Controllers
{
    public class DesignController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
