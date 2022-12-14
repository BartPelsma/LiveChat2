using System.ComponentModel.DataAnnotations;

namespace Livechat_Authentication.Models
{
    public class RegisterForm
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public virtual string Email { get; set; }
        [Required]
        public virtual string Password { get; set; }
    }
}
