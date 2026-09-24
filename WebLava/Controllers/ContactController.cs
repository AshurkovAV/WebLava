using Microsoft.AspNetCore.Mvc;
using WebLava.Models;

namespace WebLava.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitForm(ContactFormModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                    Console.WriteLine(model?.Name);
                    Console.WriteLine(model?.Emailid);
                    Console.WriteLine(model?.Msg);
                    Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>"); 

                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        // Это AJAX запрос - возвращаем JSON
                        return Json(new { success = true, message = "Сообщение успешно отправлено!" });
                    }
                    else
                    {
                        // Обычный запрос - устанавливаем TempData и редиректим
                        TempData["SuccessMessage"] = "Сообщение успешно отправлено!";
                        return RedirectToAction("Index");
                    }
                }
                catch
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = false, message = "Произошла ошибка при отправке." });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Произошла ошибка при отправке.");
                    }
                }
                

                
            }

            // Если есть ошибки валидации
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = false, message = "Пожалуйста, исправьте ошибки в форме." });
            }
            else
            {
                return View("Index", model);
            }
        }

        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }
    }
}
