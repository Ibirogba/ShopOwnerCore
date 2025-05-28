using ShopOwnerCore.Application_Core.Entities.Models;
using ShopOwnerCore.Application_Core.Entities.View_Model;
using ShopOwnerCore.Application_Core.Helper.Pager;


namespace ShopOwnerCore.Application_Core.Interface
{
    public interface IProductService
    {
       Task<IEnumerable<ProductViewModel>> GetAllProduct(Guid ShopId, ProductParameter productParameter, bool trackChanges);
       Task<IEnumerable<ProductViewModel>> GetProduct(string keyword);

       Task<ProductViewModel> GetProductById(Guid Id);

        Task CreateProduct(ProductViewModel model);


       Task CreateProductForShop(ProductViewModel model,Guid ShopId);

       Task UpdateProductInShop(ProductViewModel model,Guid ShopId,Guid ProductId);

       Task DeleteProduct(Guid Id);

    }
}
