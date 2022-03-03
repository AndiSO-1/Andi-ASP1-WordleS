using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WordleS.Models
{
    public class Game
    {
        public int Id { get; set; }

        [Required]
        public bool Finished { get; set; }

        [Required]
        public bool Win { get; set; }

        [Required]
        public int MaxAttempt { get; set; }

        [Required]
        public Word Word { get; set; }

        [Required]
        public IdentityUser User { get; set; }
    }
}
