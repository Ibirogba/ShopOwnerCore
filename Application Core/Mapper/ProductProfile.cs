using AutoMapper;
using ShopOwnerCore.Application_Core.Entities.Models;
using ShopOwnerCore.Application_Core.Entities.View_Model;


namespace ShopOwnerCore.Application_Core.Mapper
{
    public class ProductProfile:Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductViewModel>(MemberList.None).ReverseMap();
        }
    }
}
