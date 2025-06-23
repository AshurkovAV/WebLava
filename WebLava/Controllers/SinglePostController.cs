using Microsoft.AspNetCore.Mvc;

namespace WebLava.Controllers
{
    public class SinglePostController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
