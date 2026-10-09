using Microsoft.AspNetCore.Mvc;

namespace WebLava.Controllers
{
    /// <summary>
    /// Страница продукта Bakesoft — системы управления заказами и производством для кондитерских.
    /// Тот же лёгкий layout, что у лендингов CRM (Views/CrmLanding/_CrmLayout.cshtml); цены — Models/Bakesoft/BakesoftPricing.cs.
    /// </summary>
    [Route("bakesoft")]
    public class BakesoftController : Controller
    {
        [HttpGet("")]
        public IActionResult Index() => View();
    }
}
