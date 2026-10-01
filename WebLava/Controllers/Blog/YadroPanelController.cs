using Microsoft.AspNetCore.Mvc;

namespace WebLava.Controllers.Blog
{
    [Route("Blog/yadro-panel")]
    public class YadroPanelController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Blog/YadroPanel/Index.cshtml");
        }
    }
}
