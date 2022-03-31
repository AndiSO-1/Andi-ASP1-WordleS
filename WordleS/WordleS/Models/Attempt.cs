using System.ComponentModel.DataAnnotations;

namespace WordleS.Models
{
    public class Attempt
    {
        public int Id { get; set; }

        [Required]
        [StringLength(5), MinLength(5)]
        public string Value { get; set; }

        [Required]
        public int Position { get; set; }

        [Required]
        public Game Game { get; set; }
    }
}
