using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiveChat_Authentication
{
    public class AccountDTO
    {
        public AccountDTO()
        {
            //this.Dialogs = new HashSet<DialogDTO>();
        }

        [Key]
        public int AccountID { get; set; }
        [Column(TypeName = "varchar(500)")]
        public string Username { get; set; } = string.Empty;
        [Column(TypeName = "varchar(500)")]
        public virtual string Email { get; set; } = string.Empty;
        [Column(TypeName = "varchar(500)")]
        public virtual string Password { get; set; } = string.Empty;
        //[Column(TypeName = "varchar(500)")]

        //public virtual ICollection<AccountDTO> Dialogs { get; set; }

    }
}
