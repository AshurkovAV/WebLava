using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace WebLava.Models.Crm
{
    public sealed record CrmFaqItem(string Question, string Answer);

    /// <summary>
    /// JSON-LD (schema.org) для лендингов Big Lava CRM. Цены берутся из <see cref="CrmPricing"/>.
    /// Кодировщик экранирует &lt; &gt; &amp; — строку безопасно вставлять внутрь &lt;script type="application/ld+json"&gt;.
    /// </summary>
    public static class CrmSchema
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            WriteIndented = false,
        };

        public static string SoftwareApplication(string name, string description, string url, string image)
        {
            var data = new Dictionary<string, object>
            {
                ["@context"] = "https://schema.org",
                ["@type"] = "SoftwareApplication",
                ["name"] = name,
                ["description"] = description,
                ["url"] = url,
                ["image"] = image,
                ["applicationCategory"] = "BusinessApplication",
                ["operatingSystem"] = "Web, Android, iOS (через браузер)",
                ["inLanguage"] = "ru",
                ["offers"] = CrmPricing.Plans.Select(p => new Dictionary<string, object>
                {
                    ["@type"] = "Offer",
                    ["name"] = $"Тариф «{p.Name}»",
                    ["price"] = p.PricePerMonth.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    ["priceCurrency"] = CrmPricing.Currency,
                    ["url"] = url + "#pricing",
                    ["priceSpecification"] = new Dictionary<string, object>
                    {
                        ["@type"] = "UnitPriceSpecification",
                        ["price"] = p.PricePerMonth.ToString(System.Globalization.CultureInfo.InvariantCulture),
                        ["priceCurrency"] = CrmPricing.Currency,
                        ["billingDuration"] = "P1M",
                        ["unitText"] = "месяц",
                    },
                }).ToArray(),
                ["publisher"] = new Dictionary<string, object>
                {
                    ["@type"] = "Organization",
                    ["name"] = "Big Lava",
                    ["url"] = "https://biglv.ru/",
                    ["telephone"] = "+79081209023",
                    ["address"] = new Dictionary<string, object>
                    {
                        ["@type"] = "PostalAddress",
                        ["streetAddress"] = "ул. Радищева, 64",
                        ["addressLocality"] = "Курск",
                        ["addressRegion"] = "Курская область",
                        ["postalCode"] = "305004",
                        ["addressCountry"] = "RU",
                    },
                },
            };
            return JsonSerializer.Serialize(data, Options);
        }

        public static string FaqPage(IEnumerable<CrmFaqItem> items)
        {
            var data = new Dictionary<string, object>
            {
                ["@context"] = "https://schema.org",
                ["@type"] = "FAQPage",
                ["mainEntity"] = items.Select(i => new Dictionary<string, object>
                {
                    ["@type"] = "Question",
                    ["name"] = i.Question,
                    ["acceptedAnswer"] = new Dictionary<string, object>
                    {
                        ["@type"] = "Answer",
                        ["text"] = i.Answer,
                    },
                }).ToArray(),
            };
            return JsonSerializer.Serialize(data, Options);
        }
    }
}

namespace WebLava.Models.Crm
{
    /// <summary>Скриншот из wwwroot/images/crm: {Name}.webp + {Name}.png (fallback).</summary>
    public sealed record CrmImage(string Name, string Alt, int Width, int Height, bool Eager = false, string? CssClass = null);
}
