namespace WebLava.Models.Crm
{
    /// <summary>
    /// Тариф подписки Big Lava CRM.
    /// </summary>
    public sealed record CrmPlan(
        string Id,
        string Name,
        int PricePerMonth,
        string Summary,
        IReadOnlyList<string> Features,
        bool Highlighted = false)
    {
        /// <summary>Цена в формате «1 490» (неразрывный пробел между разрядами).</summary>
        public string PriceFormatted =>
            PricePerMonth.ToString("#,0", System.Globalization.CultureInfo.InvariantCulture).Replace(",", " ");
    }

    /// <summary>
    /// ЕДИНСТВЕННОЕ место, где заданы тарифы и цены Big Lava CRM.
    /// Используется в блоке тарифов на /crm/konditerskie и /crm/mebel, в выпадающем списке
    /// формы заявки и в JSON-LD (SoftwareApplication → offers).
    /// </summary>
    public static class CrmPricing
    {
        /// <summary>Длительность бесплатного пробного периода, дней.</summary>
        public const int TrialDays = 14;

        /// <summary>Валюта для разметки schema.org.</summary>
        public const string Currency = "RUB";

        // TODO(pricing): цены и лимиты пользователей — ЗАГЛУШКИ. Утвердить реальные тарифы перед запуском.
        public static readonly IReadOnlyList<CrmPlan> Plans = new[]
        {
            new CrmPlan(
                Id: "start",
                Name: "Старт",
                PricePerMonth: 1490, // TODO(pricing): заглушка
                Summary: "Для небольшой команды и домашнего производства",
                Features: new[]
                {
                    "До 3 пользователей", // TODO(pricing): уточнить лимит
                    "Заказы и сделки, канбан",
                    "Работа с телефона",
                    "Вход через Яндекс",
                }),
            new CrmPlan(
                Id: "ceh",
                Name: "Цех",
                PricePerMonth: 2990, // TODO(pricing): заглушка
                Summary: "Для цеха с несколькими сотрудниками",
                Features: new[]
                {
                    "До 10 пользователей", // TODO(pricing): уточнить лимит
                    "Всё из тарифа «Старт»",
                    "План производства",
                    "Склад и закупки",
                    "Сотрудники и роли",
                },
                Highlighted: true),
            new CrmPlan(
                Id: "proizvodstvo",
                Name: "Производство",
                PricePerMonth: 4990, // TODO(pricing): заглушка
                Summary: "Для производства с несколькими участками",
                Features: new[]
                {
                    "До 30 пользователей", // TODO(pricing): уточнить лимит
                    "Всё из тарифа «Цех»",
                    "Пароли команды",
                    "Помощь с переносом данных из Excel",
                }),
        };

        public static CrmPlan? Find(string? id) =>
            Plans.FirstOrDefault(p => string.Equals(p.Id, id, StringComparison.OrdinalIgnoreCase));

        public static int MinPrice => Plans.Min(p => p.PricePerMonth);
    }
}
