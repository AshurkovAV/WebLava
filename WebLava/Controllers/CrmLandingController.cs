using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.AspNetCore.Mvc;
using WebLava.Models.Crm;

namespace WebLava.Controllers
{
    /// <summary>
    /// Продуктовые лендинги Big Lava CRM: хаб /crm и ниши /crm/konditerskie, /crm/mebel.
    /// Отдельный лёгкий layout (Views/CrmLanding/_CrmLayout.cshtml), цены — Models/Crm/CrmPricing.cs.
    /// </summary>
    [Route("crm")]
    public class CrmLandingController : Controller
    {
        private static readonly SemaphoreSlim LeadFileLock = new(1, 1);
        private static readonly JsonSerializerOptions LeadJson = new()
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        };

        private readonly ILogger<CrmLandingController> _logger;
        private readonly IWebHostEnvironment _env;

        public CrmLandingController(ILogger<CrmLandingController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        [HttpGet("")]
        public IActionResult Index() => View();

        [HttpGet("konditerskie")]
        public IActionResult Konditerskie() => View();

        [HttpGet("mebel")]
        public IActionResult Mebel() => View();

        /// <summary>
        /// Приём заявки на пробный период. Механизм тот же, что у ContactController.SubmitForm:
        /// заявка пишется в лог приложения (stdout), AJAX получает JSON, обычный POST — редирект с TempData.
        /// Дополнительно заявка дописывается в App_Data/crm-leads.jsonl (локальный файл, без внешних сервисов).
        /// </summary>
        [HttpPost("lead")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Lead(CrmLeadModel model)
        {
            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            var product = model.Product == "mebel" ? "mebel" : "konditerskie";
            var backUrl = $"/crm/{product}#lead";

            // Honeypot: людям поле не видно; бот получает «успех», заявка не сохраняется.
            if (!string.IsNullOrWhiteSpace(model.Website))
            {
                _logger.LogWarning("CRM lead rejected by honeypot (product={Product}, ip={Ip})",
                    product, HttpContext.Connection.RemoteIpAddress);
                return Success(isAjax, backUrl);
            }

            if (!ModelState.IsValid)
            {
                if (isAjax)
                {
                    var errors = ModelState
                        .Where(kv => kv.Value?.Errors.Count > 0)
                        .ToDictionary(
                            kv => string.IsNullOrEmpty(kv.Key) ? "form" : kv.Key,
                            kv => kv.Value!.Errors.First().ErrorMessage);
                    return BadRequest(new { success = false, message = "Пожалуйста, проверьте поля формы.", errors });
                }

                TempData["CrmLeadError"] = string.Join(" ",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).Distinct());
                return Redirect(backUrl);
            }

            var plan = CrmPricing.Find(model.Plan);
            var lead = new
            {
                receivedAt = DateTimeOffset.UtcNow,
                product,
                name = model.Name.Trim(),
                phone = model.Phone.Trim(),
                email = model.Email?.Trim(),
                company = model.Company?.Trim(),
                plan = plan?.Id,
                planName = plan?.Name,
                planPricePerMonth = plan?.PricePerMonth,
                consent = model.Consent,
                ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                userAgent = Request.Headers.UserAgent.ToString(),
            };

            // 1) Как в ContactController — в лог/консоль (основной канал доставки сейчас).
            _logger.LogInformation(
                "CRM LEAD [{Product}] name={Name} phone={Phone} email={Email} company={Company} plan={Plan}",
                lead.product, lead.name, lead.phone, lead.email, lead.company, lead.planName);

            // 2) Резервная копия в локальный файл. Ошибка записи не должна ломать ответ пользователю.
            try
            {
                var dir = Path.Combine(_env.ContentRootPath, "App_Data");
                Directory.CreateDirectory(dir);
                var line = JsonSerializer.Serialize(lead, LeadJson) + Environment.NewLine;
                await LeadFileLock.WaitAsync();
                try
                {
                    await System.IO.File.AppendAllTextAsync(Path.Combine(dir, "crm-leads.jsonl"), line);
                }
                finally
                {
                    LeadFileLock.Release();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CRM lead could not be saved to App_Data/crm-leads.jsonl (lead is in the log above)");
            }

            return Success(isAjax, backUrl);
        }

        private IActionResult Success(bool isAjax, string backUrl)
        {
            var message = $"Спасибо! Заявка принята. Мы свяжемся с вами и откроем доступ на {CrmPricing.TrialDays} дней бесплатно.";
            if (isAjax)
            {
                return Json(new { success = true, message });
            }

            TempData["CrmLeadSuccess"] = message;
            return Redirect(backUrl);
        }
    }
}
