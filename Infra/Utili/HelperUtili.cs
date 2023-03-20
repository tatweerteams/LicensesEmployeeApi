using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infra.Utili
{
    public class HelperUtili
    {
        private static IHttpContextAccessor _httpContextAccessor;
        public HelperUtili(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public UserClimesDTO GetCurrentUser()
        {
            try
            {
                var currenUser = _httpContextAccessor.HttpContext.User;
                var claims = currenUser.Claims.ToList();
                if (claims.Count == 0)
                {
                    return null;
                }

                //identityClaims.AddClaim(new Claim("UserTypeState", user.UserTypeState.ToString()));

                return new UserClimesDTO
                {
                    UserID = currenUser.FindFirst(ClaimTypes.Sid)?.Value,
                    UserName = currenUser.FindFirst(ClaimTypes.Name)?.Value,
                    UserType = GetUserType(currenUser.FindFirst("UserTypeState")?.Value),
                    PhoneNumber = currenUser.FindFirst("PhoneNumber")?.Value,
                    Email = currenUser.FindFirst("Email")?.Value,
                    BankId = currenUser.FindFirst("BankId")?.Value,
                    BankName = currenUser.FindFirst("BankName")?.Value,
                    BranchId = currenUser.FindFirst("BranchId")?.Value,
                    BranchNumber = currenUser.FindFirst("BranchNumber")?.Value,
                    BranchName = currenUser.FindFirst("BranchName")?.Value,
                    RegionId = currenUser.FindFirst("RegionId")?.Value,
                    RegionName = currenUser.FindFirst("RegionName")?.Value,
                    RoleName = currenUser.FindFirst("RoleName")?.Value,
                    EmployeeNumber = currenUser.FindFirst("EmployeeNumber")?.Value,


                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        private UserTypeState? GetUserType(string accountType)
        {
            if (string.IsNullOrEmpty(accountType))
            {
                return null;
            }

            var result = int.TryParse(accountType, out int _accountType);

            if (result == true)
            {
                return (UserTypeState)_accountType;
            }
            return null;
        }


        public static bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (expires != null)
            {
                var dateTimeExpires = expires.Value.ToFileTimeUtc();
                if (DateTime.Now.ToFileTimeUtc() < dateTimeExpires)
                {
                    return true;
                }
            }
            return false;
        }

        public string RandomPassword()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(2, false));
            return builder.ToString();
        }
        public string RandomPasswordNumber()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomNumber(1000, 9999));
            return builder.ToString();
        }
        private string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }


        public string RandomOtpNumber()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomNumber(100000, 999999));
            return builder.ToString();
        }
        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }


        public string Hash(string password)
        {
            var bytes = new UTF8Encoding().GetBytes(password);
            var hashBytes = MD5.Create().ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }

        public bool VerifyHash(string inputPassword, string Passwordhash)
        {
            var hashOfInput = this.Hash(inputPassword);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return comparer.Compare(hashOfInput, Passwordhash) == 0;
        }





    }
}
