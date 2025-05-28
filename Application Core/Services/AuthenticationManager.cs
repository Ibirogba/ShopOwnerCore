using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShopOwnerCore.Application_Core.Entities.Models;
using ShopOwnerCore.Application_Core.Entities.View_Model;
using ShopOwnerCore.Application_Core.Enum;
using ShopOwnerCore.Application_Core.Interface;

namespace ShopOwnerCore.Application_Core.Services
{
    public class AuthenticationManager:IAuthenticationManager
    {
        private readonly IUserService _userService;
        private readonly IOptions<JwtSetting> _jwtSetting;

        private User _user;

        public AuthenticationManager(IUserService userService, IOptions<JwtSetting> jwtSetting)
        {
            _userService = userService;
            _jwtSetting = jwtSetting;
        }

        public async Task<bool> ValidateUser(UserAuthenticationVm model)
        {
            _user = await _userService.FindByNameAsync(model.userName);
            return (_user != null && await _userService.CheckPasswordAsync(_user, model.Password));
        }

       private SigningCredentials GetSigningCredentials()
        {
            var secret = Encoding.UTF8.GetBytes(_jwtSetting.Value.Secret);
            var key = new SymmetricSecurityKey(secret);

            return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        }

        private async Task<List<Claim>> Getclaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,_user.UserName)
            };

            var roles = await _userService.GetRoleAsync(_user);
            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials credentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken(
                issuer: _jwtSetting.Value.ValidIssuer,
                audience: _jwtSetting.Value.ValidAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSetting.Value.Expire)),
                signingCredentials: credentials
                );

            return tokenOptions;
            
             

            
        }

        public async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await Getclaims();
            var token = GenerateTokenOptions(signingCredentials,claims);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }


    }
}
