using Microsoft.AspNetCore.Mvc;

namespace WebLava.Controllers
{
    public class PortfolioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
