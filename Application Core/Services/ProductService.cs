using ShopOwnerCore.Application_Core.Interface;
using AutoMapper;
using ShopOwnerCore.Application_Core.Entities.Models;
using ShopOwnerCore.Application_Core.Entities.View_Model;
using Microsoft.EntityFrameworkCore;
using ShopOwnerCore.Application_Core.Helper.Pager;

namespace ShopOwnerCore.Application_Core.Services
{
    public class ProductService:IProductService
    {
        private readonly IUnitWork _unitWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitWork unitWork, IMapper mapper)
        {
            _unitWork = unitWork;
            _mapper = mapper;
        }

        //We can simplify this further by including our own pager
        //public async Task<PageList<ProductViewModel>> GetAllProduct(Guid ShopId, ProductParameter parameter, bool trackChanges){
        //   var Product = await _unitWork.Repository<Product>().FindByCondition(e=>e.ShopId.Equals(ShopId), trackChanges).OrderBy(e=>e.Name).ToListAsync();
        //    var ProductMapper= _mapper.Map<IEnumerable<Product>,IEnumerable<ProductViewModel>>(Product);
        // return PageList<ProductViewModel>.ToPagedList(ProductMapper,parameter.PageNumber,parameter.PageSize);
        //}
        //This solution works with smaller amount of data, but with bigger tables with millions of rows, we can improve it by modifying the GetAllProduct()
        // public async Task<PageList<ProductViewModel>> GetAllProduct(Guid ShopId,ProductParameter parameter, bool trackChanges){
        // var Product = await _unitWork.Repository<Product>().FindByCondition(e=>e.ShopId.Equals(ShopId), trackChanges).OrderBy(e=>e.Name).Skip((parameter.PageNumber-1)*parameter.PageSize)
        //     .Take(parameter.PageSize).ToListAsync();              
        //
        //var Count = Product.CountAsync();
        //var ProductMapper = _mapper.Map<IEnumerable<Product>,IEnumerable<ProductViewModel>>(Product);
        //return new PageList<ProductViewModel>(ProductMapper, parameter.PageNumber, parameter.PageSize,count);
        //}

        public async  Task<IEnumerable<ProductViewModel>> GetAllProduct(Guid ShopId, ProductParameter productParameter,bool trackChanges)
        {
            var Product = await _unitWork.Repository<Product>().FindByCondition(e => e.ShopId.Equals(ShopId), trackChanges).OrderBy(e => e.ProductName).Skip((productParameter.PageNumber - 1) * productParameter.PageSize).Take(productParameter.PageSize).ToListAsync();
            return _mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(Product);
            
        }
        //Needs Reviewing 
        public async Task<IEnumerable<ProductViewModel>> GetProduct(string keyword)
        {
            var products =  _unitWork.Repository<Product>().Query().Include(p=>p.Quantity).Where(p=>p.ProductName.Contains(keyword));
            return (IEnumerable<ProductViewModel>)products.ToList();
        }
        public async Task<ProductViewModel> GetProductById(Guid Id)
        {
            var Choice = await GetProductId(Id, true);
            return _mapper.Map<Product, ProductViewModel>(Choice);
        }
        private async Task<Product> GetProductId(Guid Id, bool trackChanges)
        {
            return await _unitWork.Repository<Product>().FindByCondition(c => c.id.Equals(Id), trackChanges).SingleOrDefaultAsync();
        }

        public async Task CreateProductForShop( ProductViewModel model, Guid ShopId)
        {
            var ProductCreate = await _unitWork.Repository<Product>().FindByCondition(p => p.ShopId.Equals(ShopId), false).SingleOrDefaultAsync();
            if(model.ShopId== ProductCreate.ShopId)
            {
                var ResultModel = _mapper.Map<Product>(model);
                await _unitWork.Repository<Product>().AddAsync(ResultModel);
            }
          
            


           
        }
        public async Task UpdateProductInShop(ProductViewModel model,Guid ShopId,Guid ProductId)
        {
            var UpdateModel = await GetProductById(model.Id);
            if (UpdateModel != null && UpdateModel.ShopId== ShopId && UpdateModel.Id== ProductId)
            {



                UpdateModel.OldUnitPrice = model.OldUnitPrice;
                UpdateModel.ProductDescription = model.ProductDescription;
                UpdateModel.UnitPrice = model.UnitPrice;
                UpdateModel.PictureUrl = model.PictureUrl;
                UpdateModel.Quantity = model.Quantity;
                UpdateModel.ProductName = model.ProductName;
                var MapModel = _mapper.Map<ProductViewModel, Product>(model);

                await _unitWork.Repository<Product>().UpdateAsync(MapModel);
            }
        }
                

        public async Task DeleteProduct(Guid Id)
        {
            var DeleteProduct = await GetProductId(Id, false);
            await _unitWork.Repository<Product>().DeleteAsync(DeleteProduct);
        }
           
        public async Task CreateProduct(ProductViewModel model)
        {
            var ResultModel = _mapper.Map<Product>(model);
            await _unitWork.Repository<Product>().AddAsync(ResultModel);
        }
            
            
        

    }
}
