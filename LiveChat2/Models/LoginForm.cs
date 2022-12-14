using System.ComponentModel.DataAnnotations;

namespace LiveChat_Authentication
{
    public class LoginForm
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
