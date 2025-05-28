using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using ShopOwnerCore.Application_Core.Entities.Models;
using ShopOwnerCore.Application_Core.Entities.View_Model;


namespace ShopOwnerCore.Application_Core.Interface
{
    public interface IUserService
    {
        #region User
        Task<User> GetCurrentUserAsync();
        string GetCurrentUserGuid();
        string GetCurrentUserName();
        string GetCurrentUserEmail();
        #endregion

        #region User info, user account
        Task<User> GetUserAsync(ClaimsPrincipal principal);
        Task<User> FindByNameAsync(string userName);

        Task<User> FindByEmailAsync(string email);

        Task<User> FindByIdAsync(string Id);

        Task<IdentityResult> CreateAsync(User user, string password);

        Task<IdentityResult> SetLockOutEnabledAsync(User user, bool enabled);

        Task<IdentityResult> ResetPasswordAsync(User user, string code, string password);

        Task<IdentityResult> ConfirmEmailAsync(User user, string code);

        Task<SignInResult> PasswordSignInAsync(string userName, string password, bool rememberMe, bool lockOutOnFailure);

        Task<ClaimsPrincipal> CreateUserPrincipal(User user);

        Task SaveResetPasswordRequest(string token, string email);

        Task<string> GenerateEmailConfirmationTokenAsync(User user);

        Task<bool> CheckPasswordAsync(User user, string Password);
        Task<string> GeneratePasswordResetTokenAsync(User user);
        Task<int> GetAccessedFailedCountAsync(User user);

        Task ToggleRequestPasswordStatusByEmail(string email);

        #endregion

        #region Roles
        Task AddUserToRolesAsync(User user, List<string> roles);
        Task AddUserRoles(string[] UserRoles);
        Task RemoveFromRolesAsync(User user, string roles);

        Task RemoveFromRolesAsync(User user, string[] roles);

        Task<List<User>> GetListRoles(string role);
        IEnumerable<string> GetCurrentUserRoles();
        Task<List<string>> GetUserRoles();

        Task<IList<string>> GetRoleAsync(User user);
        Task<IList<string>> GetUserRoleGuid(string userId);
        #endregion

        #region Validate
        Task<bool> CanSignInAsync(User user);

        Task<bool> IsEmailConfirmedAsync(User user);

        Task<bool> IsLockedOutAsync(User user);

        Task<bool> CheckValidResetPasswordToken(string token, string email);
        Task<SignInResult> CheckPasswordSignInAsync(User user, string password, bool lockoutOnFailure);

        Task SignInAsync(User user, bool isPersistent);

        Task SignOutAsync();
        bool isAuthenticated();
        #endregion


    }






}

