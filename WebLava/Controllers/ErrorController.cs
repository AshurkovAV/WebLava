using Microsoft.AspNetCore.Mvc;

namespace WebLava.Controllers
{
    // Брендированные страницы ошибок (404, 500 и др.), подключены в Program.cs
    [Route("error")]
    public class ErrorController : Controller
    {
        [Route("{code:int}")]
        public IActionResult Status(int code)
        {
            Response.StatusCode = code;
            ViewData["Code"] = code;
            return View("~/Views/Error/Status.cshtml");
        }
    }
}
