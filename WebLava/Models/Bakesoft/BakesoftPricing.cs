namespace WebLava.Models.Bakesoft
{
    /// <summary>Тариф Bakesoft: цена за 1, 6 и 12 месяцев.</summary>
    public sealed record BakesoftPlan(
        string Id,
        string Name,
        string Audience,
        string Users,
        int PriceMonth,
        int Price6Months,
        int Price12Months,
        bool Highlighted = false)
    {
        /// <summary>Экономия при оплате за год относительно помесячной оплаты, %.</summary>
        public int YearSavingPercent => (int)Math.Round(100m - Price12Months * 100m / (PriceMonth * 12));

        public static string Format(int value) =>
            value.ToString("#,0", System.Globalization.CultureInfo.InvariantCulture).Replace(",", " ");
    }

    /// <summary>
    /// Тарифы Bakesoft (bakesoft.ru) для страницы /bakesoft на biglv.ru.
    /// Источник цен — репозиторий AshurkovAV/bakesoft, Bakesoft/appsettings.json → Pricing.
    /// При изменении цен там — обновить и здесь.
    /// </summary>
    public static class BakesoftPricing
    {
        public const int TrialDays = 14;
        public const string Currency = "RUB";

        public const string SiteUrl = "https://bakesoft.ru";
        public const string RegisterUrl = "https://app.bakesoft.ru/register";
        public const string LoginUrl = "https://app.bakesoft.ru/login";

        public static readonly IReadOnlyList<BakesoftPlan> Plans = new[]
        {
            new BakesoftPlan("mini", "Мини", "Для кондитера на заказ", "До 2 человек: вы и помощник", 300, 1500, 2700),
            new BakesoftPlan("small", "Малый", "Для небольшой кондитерской", "До 5 сотрудников с ролями", 900, 4500, 8100, Highlighted: true),
            new BakesoftPlan("team", "Команда", "Для студии тортов", "До 10 сотрудников с ролями", 2200, 11200, 19800),
        };

        /// <summary>Что входит во все тарифы (функции одинаковые, отличается число сотрудников).</summary>
        public static readonly IReadOnlyList<string> CommonFeatures = new[]
        {
            "Заказы и выдача с телефона",
            "Техкарты с пересчётом под размер",
            "Себестоимость и цена",
            "Склад, закупки и план производства",
            "Уведомления в MAX",
        };
    }
}
