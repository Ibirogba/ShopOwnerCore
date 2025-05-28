using AutoMapper;
using ShopOwnerCore.Application_Core.Entities.Models;
using ShopOwnerCore.Application_Core.Entities.View_Model;



namespace ShopOwnerCore.Application_Core.Mapper
{
    public class ShopProfile:Profile
    {
        public ShopProfile()
        {
            CreateMap<Shop, ShopViewModel>(MemberList.None).ReverseMap();
            
        }
    }
}
