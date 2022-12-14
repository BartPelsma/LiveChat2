namespace LiveChat_Authentication
{
    public class Account
    {
        public int AccountID { get; set; }
        public string Username { get; set; } = string.Empty;
        public virtual string Email { get; set; } = string.Empty;
        public virtual string Password { get; set; } = string.Empty;

    }
}
