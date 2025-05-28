using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ShopOwnerCore.Application_Core.Entities.Models;
using ShopOwnerCore.Application_Core.Entities.View_Model;
using ShopOwnerCore.Application_Core.Interface;


namespace ShopOwnerCore.Application_Core.Services
{
    public class ShopService:IShopService
    {
        private readonly IUnitWork _unitWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public ShopService(IUnitWork unitWork, IMapper mapper,IUserService userService)
        {
            _unitWork = unitWork;
            _mapper = mapper;
            _userService = userService;
            
        }

        public async Task<IEnumerable<ShopViewModel>> GetAllShops()
        {
            var Shop = await _unitWork.Repository<Shop>().GetAllAsync();
            return _mapper.Map<IEnumerable<Shop>,IEnumerable<ShopViewModel >> (Shop);
        }

        public async Task CreateShop(ShopViewModel model)
        {
            
            
            var Result = _mapper.Map<Shop>(model);
            await _unitWork.Repository<Shop>().AddAsync(Result);
            
        }

        private async Task<Shop> GetById(Guid Id, bool TrackChanges)
        {
            return await _unitWork.Repository<Shop>().FindByCondition(e => e.id.Equals(Id), false).SingleOrDefaultAsync();
        }

        public async Task<ShopViewModel> GetShopById(Guid Id)
        {
            var Shop = await GetById(Id, true);
            return _mapper.Map<Shop, ShopViewModel>(Shop);
        }

        public async Task DeleteShop(Guid ShopId)
        {
            var ShopContainer = await GetById(ShopId,false);
            await _unitWork.Repository<Shop>().DeleteAsync(ShopContainer);
        }

       
       


    }
}
