using Ad.Core.Configurations;
using Ad.Core;
using Ad.Core.Models;
using Ad.Core.Response;
using Ad.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Ad.Core.Constants;

namespace Ad.Service
{
    public class AuthService : IAuthService
    {
        private readonly JwtSetting _jwtSetting;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            IUnitOfWork unitOfWork,
            IOptions<JwtSetting> jwtSetting)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _jwtSetting = jwtSetting.Value;
        }
        public async Task<OperationResponse<object>> Login(string email, string password, Guid tenantId)
        {
            var user = _userManager.Users.SingleOrDefault(x => x.Email == email && x.Password == password);

            if (user == null)
                return new OperationResponse<object>("Username or Password is incorrect");

            if (!await _userManager.CheckPasswordAsync(user, password))
                return new OperationResponse<object>("Username or Password is incorrect");

            var result = await GenerateAuthToken(user);
            var data = new
            {
                Token = result.Item1,
                UserId = user.Id,
                Name = user.FirstName + ' ' + user.LastName,
                user.Email,
                TenantId = tenantId,
                user.AccountNumber,
                user.BVN,
                user.Balance
            };

            return new OperationResponse<object>("login successful", data, true);
        }


        public async Task<OperationResponse<ApplicationUser>> Register(ApplicationUser user, string password, Guid tenantId)
        {
            if (_userManager.Users.Any(x => x.Email.ToLower() == user.Email.ToLower() && x.TenantId == tenantId))
                return new OperationResponse<ApplicationUser>("Account with this email already exists!");

            user.TenantId = tenantId;
            user.UserName = string.Concat(tenantId, "-", user.Email);
            user.AccountNumber = GenerateAccountNumber();
            user.BVN = GenerateBVNNumber();
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                return new OperationResponse<ApplicationUser>(string.Join(",", result.Errors.Select(x => x.Description)));

            var userProfile = new UserProfile
            {
                ProfileId = user.Id,
                TenantId = user.TenantId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                AccountNumber = user.AccountNumber,
            };

            await _unitOfWork.UserProfiles.AddAsync(userProfile);
            //await _unitOfWork.CommitChangesAsync();
            return new OperationResponse<ApplicationUser>("registration successful", user, true);
        }


        private async Task<(string,List<ControllerServiceModel>)> GenerateAuthToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSetting.Secret);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(CustomClaimTypes.TenantId, user.TenantId.ToString())
            };

            var userRole = _userManager.GetRolesAsync(user).Result.SingleOrDefault();
            //var providerMenus = new List<ProviderMenu>();
            var permissions = new List<ControllerServiceModel>();

            if (!string.IsNullOrWhiteSpace(userRole))
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = _roleManager.FindByNameAsync(userRole).Result;
                var roleMenus = !string.IsNullOrWhiteSpace(role.Menus) ? JsonSerializer.Deserialize<List<RoleProviderMenuServiceModel>>(role?.Menus) : new List<RoleProviderMenuServiceModel>();

                permissions = !string.IsNullOrWhiteSpace(role.Permissions) ? JsonSerializer.Deserialize<List<ControllerServiceModel>>(role.Permissions) : permissions;
            }


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return (tokenString, permissions);
        }

        private string GenerateAccountNumber()
        {
            Random rnd = new Random();

            int firstDigit = rnd.Next(0, 10);

            string remainingDigits = string.Concat(Enumerable.Range(0, 9).Select(n => rnd.Next(0, 10).ToString()));

            return firstDigit.ToString() + remainingDigits;
        }


        private string GenerateBVNNumber()
        {
            Random rnd = new Random();

            int firstDigit = rnd.Next(0, 11);

            string remainingDigits = string.Concat(Enumerable.Range(0, 10).Select(n => rnd.Next(0, 11).ToString()));

            return firstDigit.ToString() + remainingDigits;
        }
    }
}
