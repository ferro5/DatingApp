using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.DTO
{
    public class UserDto
    {
        public int Id { get; set;  }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [StringLength(8,MinimumLength =4,ErrorMessage ="You mast specify password between 4 and 8 characters")]
        public string Password { get; set; }
    }
}
