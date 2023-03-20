using CollactionData.DTOs;
using CollactionData.Models.AuthUserModel;
using Domain;
using FilterAttributeWebAPI.Common;
using Infra;
using Infra.Utili;
using Infra.Utili.ConfigrationModels;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IdentityServices.services
{
    public interface IAuthUserServices
    {

        Task<UserAuthDTO> CheckUserAccess(string nameOrNumber, string password);
        Task<UserAuthDTO> CheckIsAdminSystem(string userName, string password);
        Task<UserAuthDTO> GetUserInfo(string userId);
        Task<string> SingIn(UserAuthDTO user);
        Task<bool> ChengeUserPassword(ChengePasswordModel model);
    }

    public class AuthUserServices : IAuthUserServices
    {

        private readonly IOptions<AppSettingsConfig> _settings;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOptions<UserSystemDTO> _userSystem;
        private readonly HelperUtili _helper;

        public AuthUserServices(IOptions<AppSettingsConfig> settings, IUnitOfWork unitOfWork, IOptions<UserSystemDTO> userSystem, HelperUtili helper)
        {
            _settings = settings;
            _unitOfWork = unitOfWork;
            _userSystem = userSystem;
            _helper = helper;
        }

        public async Task<UserAuthDTO> CheckIsAdminSystem(string userName, string password)
        {
            var adminSystem = _userSystem.Value;
            UserAuthDTO userAuth = new();
            if (adminSystem != null)
            {

                if (adminSystem.Name.Equals(userName) && adminSystem.Password.Equals(password))
                {
                    userAuth = new()
                    {
                        Name = adminSystem?.Name,
                        Permisstions = new List<string>() { adminSystem?.Permisstion },
                        RoleName = adminSystem?.Name,
                        Id = string.Empty,
                        RoleId = "",
                        UserTypeState = UserTypeState.SuperAdmin,
                        //IsFirstLogin = false,
                    };
                    var createToken = await SingIn(userAuth);
                    if (string.IsNullOrEmpty(createToken))
                    {
                        userAuth.CheckIsAdminState = CheckIsAdminState.IsErrorCreateToken;
                        return userAuth;
                    }

                    userAuth.AccessToken = createToken;
                    userAuth.CheckIsAdminState = CheckIsAdminState.IsAdmin;
                }
            }

            return userAuth;

        }

        public async Task<UserAuthDTO> CheckUserAccess(string nameOrNumber, string password)
        => (await _unitOfWork.GetRepositoryReadOnly<User>().FindBy(
                        predicate: pred =>
                        (pred.Email.Equals(nameOrNumber) || pred.EmployeeNumber.Equals(nameOrNumber)) &&
                            pred.PasswordHash.Equals(password),
                        selector: select => new UserAuthDTO
                        {
                            Name = select.Name,
                            Id = select.Id,
                            PhoneNumber = select.PhoneNumber,
                            RoleId = select.RoleId,
                            Permisstions = select.Role.RolePermisstions.
                                    Where(pred => pred.Role.IsActive == true && pred.Permisstion.IsActive == true
                                   ).Select(select => select.Permisstion.Name).ToList(),
                            RoleName = select.Role.Name,
                            BranchName = select.BranchName,
                            BranchId = select.BranchId,
                            BranchNumber = select.BranchNumber,
                            BankId = select.BankId,
                            BankName = select.BankName,
                            RegionId = select.RegionId,
                            RegionName = select.RegionName,
                            Email = select.Email,
                            UserTypeState = select.Role.UserType,
                            EmployeeNumber = select.EmployeeNumber,
                            IsActive = select.IsActive,
                            IsFirstLogin = select.IsFirstLogin,
                        }
                    )).SingleOrDefault();

        public async Task<bool> ChengeUserPassword(ChengePasswordModel model)
        {
            var result = await _unitOfWork.GetRepositoryReadOnly<User>().GetByID(model.UserId);
            if (result == null)
                throw new ApplicationEx("يرجي إعادة المحاولة لاحقا");

            if (!string.IsNullOrEmpty(result.EmployeeNumber) && !result.EmployeeNumber.Equals(model.EmployeeNumber))
                throw new ApplicationEx("رقم الوظيفي غير مطابق");

            var chekEmployeeNo = await _unitOfWork.GetRepositoryReadOnly<User>().
                AnyAsync(a => !result.Id.Equals(model.UserId) && a.EmployeeNumber.Equals(model.EmployeeNumber));

            if (chekEmployeeNo)
                throw new ApplicationEx("رقم الوظيفي موجود مسبقا");

            result.PasswordHash = _helper.Hash(model.Password);
            result.EmployeeNumber = model.EmployeeNumber;
            result.IsFirstLogin = true;

            await _unitOfWork.SaveChangeAsync();

            return true;
        }

        public async Task<UserAuthDTO> GetUserInfo(string userId)
            => (await _unitOfWork.GetRepositoryReadOnly<User>().FindBy(
                        predicate: pred => pred.Id.Equals(userId),
                        selector: select => new UserAuthDTO
                        {
                            Name = select.Name,
                            Id = select.Id,
                            PhoneNumber = select.PhoneNumber,
                            RoleId = select.RoleId,
                            Permisstions = select.Role.RolePermisstions.
                                    Where(pred => pred.Role.IsActive == true && pred.Permisstion.IsActive == true
                                   ).Select(select => select.Permisstion.Name).ToList(),
                            RoleName = select.Role.Name,
                            BranchName = select.BranchName,
                            BranchId = select.BranchId,
                            BranchNumber = select.BranchNumber,
                            BankId = select.BankId,
                            BankName = select.BankName,
                            RegionId = select.RegionId,
                            RegionName = select.RegionName,
                            UserTypeState = select.Role.UserType,
                            EmployeeNumber = select.EmployeeNumber,
                            IsActive = select.IsActive,
                            IsFirstLogin = select.IsFirstLogin,
                        }
                    )).SingleOrDefault();



        public async Task<string> SingIn(UserAuthDTO user)
        {
            await Task.FromResult(true);
            DateTime expired = DateTime.UtcNow.AddHours(_settings.Value.AddHourExpired);
            DateTime issuedAt = DateTime.UtcNow;

            var claims = addClaimsIdentity(user, expired);
            return await CreateToken(issuedAt, expired, claims);
        }

        private ClaimsIdentity addClaimsIdentity(UserAuthDTO user, DateTime expired)
        {
            ClaimsIdentity identityClaims = new ClaimsIdentity();
            identityClaims.AddClaim(new Claim(ClaimTypes.Sid, user.Id));
            identityClaims.AddClaim(new Claim(ClaimTypes.Name, user.Name));
            identityClaims.AddClaim(new Claim(ClaimTypes.Hash, Guid.NewGuid().ToString()));
            identityClaims.AddClaim(new Claim(ClaimTypes.Expired, expired.ToString()));
            identityClaims.AddClaim(new Claim("PhoneNumber", user.PhoneNumber?.ToString() ?? string.Empty));
            identityClaims.AddClaim(new Claim("BranchNumber", user?.BranchNumber?.ToString() ?? string.Empty));
            identityClaims.AddClaim(new Claim("Email", user.Email?.ToString() ?? string.Empty));
            identityClaims.AddClaim(new Claim("RoleName", user.RoleName?.ToString() ?? string.Empty));
            identityClaims.AddClaim(new Claim("BankId", user.BankId?.ToString() ?? string.Empty));
            identityClaims.AddClaim(new Claim("BankName", user.BankName?.ToString() ?? string.Empty));
            identityClaims.AddClaim(new Claim("RegionId", user.RegionId?.ToString() ?? string.Empty));
            identityClaims.AddClaim(new Claim("RegionName", user.RegionName?.ToString() ?? string.Empty));
            identityClaims.AddClaim(new Claim("BranchId", user.BranchId?.ToString() ?? string.Empty));
            identityClaims.AddClaim(new Claim("BranchName", user.BranchName?.ToString() ?? string.Empty));
            identityClaims.AddClaim(new Claim("EmployeeNumber", user.EmployeeNumber?.ToString() ?? string.Empty));
            identityClaims.AddClaim(new Claim("UserTypeState", ((int)user.UserTypeState).ToString()));

            foreach (var itemPermissions in user?.Permisstions)
            {
                identityClaims.AddClaim(new Claim(ClaimTypes.Role, itemPermissions));
            }

            return identityClaims;

        }

        private async Task<string> CreateToken(DateTime issuedAt, DateTime expired, ClaimsIdentity identityClaims)
        {
            await Task.FromResult(true);
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(_settings.Value.Secret));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            TokenValidationParameters validationParameters = new TokenValidationParameters()
            {
                ValidAudience = _settings.Value.ValidAudience,
                ValidIssuer = _settings.Value.ValidIssuer,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                LifetimeValidator = HelperUtili.LifetimeValidator,
                IssuerSigningKey = securityKey,
            };
            var createToken = tokenHandler.CreateJwtSecurityToken(issuer: "", audience: "",
                       subject: identityClaims, notBefore: issuedAt, expires: expired, signingCredentials: signingCredentials);

            return tokenHandler.WriteToken(createToken);
        }


    }
}


