using Microsoft.AspNetCore.Mvc;

namespace WebLava.Controllers
{
    public class ServiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
