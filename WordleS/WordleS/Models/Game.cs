using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WordleS.Models
{
    public partial class Game
    {
        public int Id { get; set; }

        [Required]
        public bool Finished { get; set; }

        [Required]
        public bool Win { get; set; }

        [Required]
        public int MaxAttempt { get; set; }

        public int Duration { get; set; }

        [Required]
        public Word Word { get; set; }

        [Required]
        public IdentityUser User { get; set; }

        public ICollection<Attempt> Attempt { get; set; }
    }
}
