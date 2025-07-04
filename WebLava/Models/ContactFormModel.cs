using System.ComponentModel.DataAnnotations;

namespace WebLava.Models
{
    public class ContactFormModel
    {
        [Required(ErrorMessage = "Пожалуйста, введите ваше имя")]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите ваш email")]
        [EmailAddress(ErrorMessage = "Пожалуйста, введите корректный email")]
        [Display(Name = "Email")]
        public string Emailid { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите сообщение")]
        [Display(Name = "Сообщение")]
        public string Msg { get; set; }
    }
}
