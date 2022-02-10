using System.ComponentModel.DataAnnotations;

namespace WordleS.Models
{
    public class Users
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string? login { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string? pseudo { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 12)]
        public string? password { get; set; }
    }
}
