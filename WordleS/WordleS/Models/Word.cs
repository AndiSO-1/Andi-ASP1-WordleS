using System.ComponentModel.DataAnnotations;

namespace WordleS.Models
{
    public class Word
    {
        public int Id { get; set; }

        [Required]
        [StringLength(5), MinLength(5)]
        public string Value { get; set; }

        [Required]
        public ICollection<Game> Game { get; set; }
    }
}
