using Microsoft.AspNetCore.Mvc;

namespace WebLava.Controllers.Blog
{
    [Route("Blog/crm-design")]
    public class CrmDesignController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Blog/CrmDesign/Index.cshtml");
        }
    }
}
