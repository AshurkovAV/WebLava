using Microsoft.AspNetCore.Mvc;

namespace WebLava.Controllers.Blog
{
    [Route("Blog/DevAndroid")]
    public class DevAndroidController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Blog/DevAndroid/Index.cshtml");
        }
    }
}
