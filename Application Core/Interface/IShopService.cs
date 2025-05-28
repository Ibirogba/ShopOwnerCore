using ShopOwnerCore.Application_Core.Entities.Models;
using ShopOwnerCore.Application_Core.Entities.View_Model;


namespace ShopOwnerCore.Application_Core.Interface
{
    public interface IShopService
    {
        //Create, Update, Delete, Get_All,Get_by_Id,create product in shop, 
        Task<IEnumerable<ShopViewModel>> GetAllShops();
        
        Task CreateShop(ShopViewModel viewModel);

        Task DeleteShop(Guid ShopId);

        Task<ShopViewModel> GetShopById(Guid ShopId);



        

    }
}
