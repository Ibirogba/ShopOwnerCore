using ShopOwnerCore.Application_Core.Entities.View_Model;

namespace ShopOwnerCore.Application_Core.Interface
{
    public interface IAuthenticationManager
    {
        Task<bool> ValidateUser(UserAuthenticationVm model);
        Task<string> CreateToken();
    }
}
