using AutoMapper;
using ShopOwnerCore.Application_Core.Entities.Models;
using ShopOwnerCore.Application_Core.Entities.View_Model;

namespace ShopOwnerCore.Application_Core.Mapper
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserViewModel>(MemberList.None).ReverseMap();
            CreateMap<User, UserAuthenticationVm>().ReverseMap();
        }
    }
}
