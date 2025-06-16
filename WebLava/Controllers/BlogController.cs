using Microsoft.AspNetCore.Mvc;

namespace WebLava.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
