using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApp.Models;
using Newtonsoft.Json;

namespace WebApp.Models
{
    public class CardSet
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public int UserId { get; set; } // UserId теперь nullable

        public virtual User User { get; set; } // User теперь необязательный

        [JsonIgnore]
        public List<Card>? Cards { get; set; }
    }
}