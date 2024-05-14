using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Логин обязателен")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Пароль обязателен")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Имя обязательно")]
        public string Name { get; set; }
        public List<CardSet>? CardSets { get; set; }
    }
}