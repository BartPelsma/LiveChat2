namespace LiveChat_Authentication
{
    public class AccountDTOC
    {
        public AccountDTO ModelToDto(Account model)
        {
            AccountDTO dto = new AccountDTO()
            {
                AccountID = model.AccountID,
                Username = model.Username,
                Email = model.Email,
                Password = model.Password,
            };

            return dto;
        }

        public Account DtoToModel(AccountDTO dto)
        {
            Account model = new Account()
            {
                AccountID = dto.AccountID,
                Username = dto.Username,
                Email = dto.Email,
                Password = dto.Password,
            };

            return model;
        }
    }
}
