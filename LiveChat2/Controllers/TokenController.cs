using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using LiveChat_Authentication;

namespace Livechat_Authentication
{
    public class TokenController : Controller
    {
        private const string SECRET_KEY = "this is my custom Secret key for authnetication";
        public static readonly SymmetricSecurityKey SIGNING_KEY = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenController.SECRET_KEY));

        public string GenerateToken(Account account)
        {
            var token = new JwtSecurityToken(
                claims: new Claim[]
                {
                    new Claim("AccountID", Convert.ToString(account.AccountID)),
                    new Claim("Email", account.Email),
                    new Claim("Username", account.Username)
                },
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: new SigningCredentials(SIGNING_KEY, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token).ToString();
        }

        [Authorize]
        public bool isExpired(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);
                handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SECRET_KEY)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return false;
            }
            catch
            {
                return true;
            }
        }

        public bool IsTokenExpired(string token)
        {
            try
            {
                if (token == null || ("").Equals(token))
                {
                    return true;
                }

                int indexOfFirstPoint = token.IndexOf('.') + 1;
                String toDecode = token.Substring(indexOfFirstPoint, token.LastIndexOf('.') - indexOfFirstPoint);
                while (toDecode.Length % 4 != 0)
                {
                    toDecode += '=';
                }

                //Decode the string
                string decodedString = Encoding.ASCII.GetString(Convert.FromBase64String(toDecode));

                //Get the "exp" part of the string
                Regex regex = new Regex("(\"exp\":)([0-9]{1,})");
                Match match = regex.Match(decodedString);
                long timestamp = Convert.ToInt64(match.Groups[2].Value);

                DateTime date = new DateTime(1970, 1, 1).AddSeconds(timestamp);
                DateTime compareTo = DateTime.UtcNow;

                int result = DateTime.Compare(date, compareTo);

                return result < 0;
            }
            catch
            {
                return true;
            }
        }

        public List<Claim> readOutToken(string token)
        {
            try
            {
                List<Claim> data = new List<Claim>();

                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);

                foreach (Claim c in jwtSecurityToken.Claims)
                {
                    data.Add(c);
                }
                return data;
            }
            catch
            {
                return null;
            }
        }

        public Account TokenToAccount(string token)
        {
            try
            {
                List<Claim> data = new List<Claim>();

                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);

                foreach (Claim c in jwtSecurityToken.Claims)
                {
                    data.Add(c);
                }

                Account account = new Account();
                account.AccountID = Convert.ToInt32(data.FirstOrDefault(c => c.Type == "AccountID").Value);
                account.Email = data.FirstOrDefault(c => c.Type == "Email").Value;
                account.Username = data.FirstOrDefault(c => c.Type == "Username").Value;

                return account;
            }
            catch
            {
                return null;
            }
        }
    }
}
