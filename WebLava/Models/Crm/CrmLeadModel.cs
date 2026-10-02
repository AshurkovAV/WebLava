using System.ComponentModel.DataAnnotations;

namespace WebLava.Models.Crm
{
    /// <summary>
    /// Заявка на пробный период Big Lava CRM с лендингов /crm/konditerskie и /crm/mebel.
    /// </summary>
    public class CrmLeadModel : IValidatableObject
    {
        /// <summary>Лендинг, с которого пришла заявка: «konditerskie» или «mebel».</summary>
        [Required]
        [RegularExpression("^(konditerskie|mebel)$", ErrorMessage = "Неизвестный продукт")]
        public string Product { get; set; } = "";

        [Required(ErrorMessage = "Укажите, как к вам обращаться")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Имя должно быть от 2 до 100 символов")]
        [Display(Name = "Имя")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Укажите телефон для связи")]
        [RegularExpression(@"^\+?[\d\s()\-]{10,25}$", ErrorMessage = "Проверьте номер телефона")]
        [Display(Name = "Телефон")]
        public string Phone { get; set; } = "";

        [EmailAddress(ErrorMessage = "Проверьте адрес почты")]
        [StringLength(150)]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [StringLength(150, ErrorMessage = "Слишком длинное название")]
        [Display(Name = "Компания или вид бизнеса")]
        public string? Company { get; set; }

        [Display(Name = "Тариф")]
        public string? Plan { get; set; }

        [Display(Name = "Согласие на обработку персональных данных")]
        [Range(typeof(bool), "true", "true", ErrorMessage = "Нужно согласие на обработку персональных данных")]
        public bool Consent { get; set; }

        /// <summary>Поле-ловушка для ботов (скрыто от людей). Должно оставаться пустым.</summary>
        public string? Website { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var digits = new string((Phone ?? "").Where(char.IsDigit).ToArray());
            if (!string.IsNullOrWhiteSpace(Phone) && (digits.Length < 10 || digits.Length > 15))
            {
                yield return new ValidationResult("Проверьте номер телефона", new[] { nameof(Phone) });
            }

            if (!string.IsNullOrEmpty(Plan) && CrmPricing.Find(Plan) is null)
            {
                yield return new ValidationResult("Выберите тариф из списка", new[] { nameof(Plan) });
            }
        }
    }
}
