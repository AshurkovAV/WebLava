using Microsoft.AspNetCore.Mvc;

namespace WebLava.Controllers
{
    public class PricingPlanController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
