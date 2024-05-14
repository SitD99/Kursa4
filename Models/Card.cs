using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;


namespace WebApp.Models
{
    public class Card
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Текст карточки обязателен")]
        public string FrontText { get; set; }

        [Required(ErrorMessage = "Текст карточки обязателен")]
        public string BackText { get; set; }

        public int CardSetId { get; set; }
        [JsonIgnore]
        public CardSet CardSet { get; set; }
    }
}