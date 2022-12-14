using Livechat_Authentication.Models;
using LiveChat_Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Livechat_Authentication
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        TokenController TC = new TokenController();
        private readonly ApplicationDbContext _dbContext;
        public AccountController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("Loginform")]
        public async Task<IActionResult> Login(LoginForm loginForm)
        {
            try
            {
                //Check Input
                if (string.IsNullOrWhiteSpace(loginForm.Email))
                {
                    return BadRequest("Missing_Email");
                }
                if (string.IsNullOrWhiteSpace(loginForm.Password))
                {
                    return BadRequest("Missing_Password");
                }

                //Create Variables
                var container = new AccountContainer(_dbContext);
                var token = "";

                //Check if account Exists
                var account = container.Login(loginForm);

                if (account == null || account.AccountID == 0)
                {
                    return BadRequest("Account_Not_Found");
                }
                else
                {
                    //Generate Token
                    token = TC.GenerateToken(account).ToString();
                    return Ok(new
                    {
                        accountname = account.Username,
                        token = token,
                    });
                }
            }
            catch
            {
                return BadRequest("Error_When_Getting_Account");
            }
        }

        [HttpPost("Registerform")]
        public async Task<IActionResult> Register(RegisterForm registerForm)
        {
            if (string.IsNullOrWhiteSpace(registerForm.Email) || string.IsNullOrWhiteSpace(registerForm.Email) || string.IsNullOrWhiteSpace(registerForm.Email))
            {
                return BadRequest("Missing_Data");
            }
            else
            {
                var user = _dbContext.accounts.Where(x => x.Email.Equals(registerForm.Email)).FirstOrDefault();

                if (user == null)
                {
                    try
                    {
                        AccountDTO accountDTO = new AccountDTO()
                        {
                            Email = registerForm.Email,
                            Username = registerForm.Username,
                            Password = registerForm.Password,
                        };

                        _dbContext.accounts.Add(accountDTO);
                        _dbContext.SaveChanges();
                        return Ok("User_Saved");
                    }
                    catch
                    {
                        return BadRequest("Error_While_Saving_User");
                    }
                }
                else
                {
                    return BadRequest("Email_Already_Used");
                }
            }

        }

        [Route("/[controller]/CheckToken")]
        [HttpPost]
        public async Task<IActionResult> CheckToken(string token)
        {
            try
            {
                //Check Input
                if (token == null)
                {
                    return BadRequest("Missing_Token");
                }

                //Check if token is expired
                if (TC.isExpired(token) == true)
                {
                    return BadRequest("Token_Expired_Or_Not_Valid");
                }

                //Get Account from Token
                Account account = TC.TokenToAccount(token);
                if (account.AccountID == 0)
                {
                    return BadRequest("No_Valid_AccountID");
                }

                //Refresh Token
                string newToken = TC.GenerateToken(account);
                Response.Headers.Add("AuthenticationToken", newToken);

                return Ok(account);
            }
            catch
            {
                return BadRequest("Error_When_Getting_Account");
            }
        }
    }
}
