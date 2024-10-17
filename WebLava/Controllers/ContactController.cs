using Microsoft.AspNetCore.Mvc;

namespace WebLava.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
