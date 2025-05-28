using ShopOwnerCore.Application_Core.Interface;
using ShopOwnerCore.Application_Core.Entities.View_Model;
using ShopOwnerCore.Application_Core.Entities.Models;
using ShopOwnerCore.Application_Core.Data;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Identity;
using ShopOwnerCore.Application_Core.Enum;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.Identity.Client;


namespace ShopOwnerCore.Application_Core.Services
{
    public class UserService:IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitWork _unitWork;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        private readonly string _currentUserGuid;
        private readonly string _currentUserName;
        private readonly string _currentUserEmail;

        public UserService(IHttpContextAccessor httpContextAccessor,IUnitWork unitWork, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<ApplicationRole> roleManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitWork = unitWork;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _currentUserGuid = _httpContextAccessor?.HttpContext?.User?.FindFirst(UserClaimsKey.Sub)?.Value;
            _currentUserName = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            _currentUserEmail = _currentUserGuid == null ? "" : userManager.FindByIdAsync(_currentUserGuid)?.Result?.Email;
            
        }

        public UserService()
        {
            _httpContextAccessor = new HttpContextAccessor();
        }
        #region User
        public async Task<User> GetCurrentUserAsync()
        {
            return await _unitWork.Repository<User>().GetByUniqueIdAsync(_currentUserGuid);

        }

        public async Task<IEnumerable<User>> GetAllUser()
        {
            return await _unitWork.Repository<User>().GetAll().AsQueryable().ToListAsync();
        }

       public async Task<IEnumerable<string>> GetAllUserIds()
        {
            return await _unitWork.Repository<User>().Query().Select(u => u.Id).ToListAsync();
        }

        public string GetCurrentUserGuid()
        {
            return _currentUserGuid;
        }

        public string GetCurrentUserName()
        {
            return _currentUserName;
        }
        public string GetCurrentUserEmail()
        {
            return _currentUserEmail;
        }

        #endregion

        #region User info, user account
        public async Task<User> GetUserAsync(ClaimsPrincipal principal)
        {
            return await _userManager.GetUserAsync(principal);
        }

        public  async Task<User> FindByNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<User> FindByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<IdentityResult> CreateAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);

        } 

        public async Task<IdentityResult> SetLockOutEnabledAsync(User user, bool enabled)
        {
            return await _userManager.SetLockoutEnabledAsync(user, enabled);
        }

        public async Task<IdentityResult> ResetPasswordAsync(User user, string code, string password)
        {
            return await _userManager.ResetPasswordAsync(user, code, password);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string code)
        {
            return await _userManager.ConfirmEmailAsync(user, code);
        }

        public async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool rememberMe, bool lockoutOnFailure)
        {
            return await _signInManager.PasswordSignInAsync(userName, password, rememberMe, lockoutOnFailure);
        }

        public async Task<ClaimsPrincipal> CreateUserPrincipal(User user)
        {
            return await _signInManager.CreateUserPrincipalAsync(user);
        }

        public async Task SaveResetPasswordRequest(string token, string email)
        {
            var passwordRequest = new PasswordRequest
            {
                Token = token,
                Email = email,
                isActive= true,

            };
            await _unitWork.Repository<PasswordRequest>().AddAsync(passwordRequest);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<int> GetAccessedFailedCountAsync(User user)
        {
            return await _userManager.GetAccessFailedCountAsync(user);
        }

        public async Task ToggleRequestPasswordStatusByEmail(string email)
        {
            var passwordRequest = await _unitWork.Repository<PasswordRequest>().Query().Where(rq => rq.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && rq.isActive).ToListAsync();

            foreach(var password in passwordRequest)
            {
                password.isActive = false;
                await _unitWork.Repository<PasswordRequest>().UpdateAsync(password);
            }
        }

        public async Task<bool> CheckPasswordAsync(User user, string Password)
        {
            return await _userManager.CheckPasswordAsync(user, Password);
        }
        #endregion

        #region Roles

        public async Task AddUserToRolesAsync(User user, List<string> roles)
        {
            await _userManager.AddToRolesAsync(user, roles);
        }

        public async Task AddUserRoles(string[] userRoles)
        {
            foreach(var role in userRoles)
            {
                if(!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new ApplicationRole
                    {
                        Name = role,
                        NormalizedName = role.ToUpper(),
                    });
                }
            }
        }

        public async Task RemoveFromRolesAsync(User user, string roles)
        {
            await _userManager.RemoveFromRoleAsync(user, roles);
                
        }

        public async Task RemoveFromRolesAsync(User user , string[] roles)
        {
            await _userManager.RemoveFromRolesAsync(user, roles);
        }
        
        public IEnumerable<string> GetCurrentUserRoles()
        {
            var claims = _httpContextAccessor.HttpContext.User.Claims.ToList();
            foreach(var claim in claims)
            {
                if (claim.Type == UserClaimsKey.Role)
                    yield return claim.Value;
            }
        }

        public async Task<List<string>> GetUserRoles()
        {
            return await _roleManager.Roles.Select(x => x.Name).ToListAsync();
        }

        public async Task<IList<string>> GetUserRoleGuid(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<IList<string>> GetRoleAsync(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<List<User>> GetListRoles(string Role)
        {
            var userList = await _userManager.GetUsersInRoleAsync(Role);
            return userList.ToList();
        }
        #endregion

        #region Validate
        public async Task<bool> CanSignInAsync(User user)
        {
            return await _signInManager.CanSignInAsync(user);
        }

        public async Task<bool> IsEmailConfirmedAsync(User user)
        {
            return await _userManager.IsEmailConfirmedAsync(user);
        }

        public async Task<bool> IsLockedOutAsync(User user)
        {
            return await _userManager.IsLockedOutAsync(user);
        }

        public async Task<bool> CheckValidResetPasswordToken(string token, string email)
        {
            var passwordRequest = await _unitWork.Repository<PasswordRequest>().Query().Where(rq => rq.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && rq.Token.Equals(token, StringComparison.OrdinalIgnoreCase) && rq.isActive).SingleOrDefaultAsync();
            return passwordRequest != null;
        }

        public async Task<SignInResult> CheckPasswordSignInAsync(User user, string password, bool lockoutOnFailure)
        {
            return await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure);
        }

        public async Task SignInAsync(User user, bool isPersistent)
        {
            await _signInManager.SignInAsync(user, isPersistent);
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public bool isAuthenticated()
        {
            return _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }
        #endregion
    }
}
