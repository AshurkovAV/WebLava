using Microsoft.AspNetCore.Mvc;

namespace WebLava.Controllers.Blog
{
    [Route("Blog/Mvp")]
    public class MvpController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Blog/Mvp/Index.cshtml");
        }
    }
}
