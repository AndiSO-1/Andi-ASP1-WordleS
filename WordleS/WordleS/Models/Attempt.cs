using System.ComponentModel.DataAnnotations;

namespace WordleS.Models
{
    public class Attempt
    {
        public int Id { get; set; }

        [Required]
        public bool Value { get; set; }

        [Required]
        public bool Position { get; set; }

        [Required]
        public Game Game { get; set; }
    }
}
