namespace LiveChat_Authentication
{
    public class AccountContainer
    {
        private readonly ApplicationDbContext _dbContext;

        public AccountContainer(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Account Login(LoginForm loginAccount)
        {
            try
            {
                //Create Variables
                var converter = new AccountDTOC();

                //Get Account
                var accountDTO = _dbContext.accounts.FirstOrDefault(a => a.Email == loginAccount.Email && a.Password == loginAccount.Password);

                if (accountDTO == null)
                {
                    return null;
                }

                var account = converter.DtoToModel(accountDTO);
                return account;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
