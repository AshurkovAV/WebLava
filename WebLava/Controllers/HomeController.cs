using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebLava.Controllers
{
    public class HomeController : Controller
    {
        // GET: HomeController
        public ActionResult Index()
        {
            Console.WriteLine($"Пришли на страницу {DateTime.Now}");
            try
            {
                //// Получение IP-адреса
                //var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

                //// Если за прокси (например, Nginx)
                //if (HttpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
                //{
                //    ipAddress = forwardedFor.FirstOrDefault()?.Split(',').FirstOrDefault()?.Trim();
                //}

                //// User-Agent браузера
                //var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

                //Console.WriteLine($"IP: {ipAddress}, Browser: {userAgent}");
            }
            catch { }   
           
            return View("Index");
        }

        // GET: HomeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HomeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HomeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HomeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
