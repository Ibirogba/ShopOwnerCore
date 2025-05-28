namespace ShopOwnerCore.Application_Core.Entities.Models
{
    public class Product:BaseModel
    {
        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal OldUnitPrice { get; set; }

        public string PictureUrl { get; set; }

        //Navigation Properties because a Product is link to a shop or Collections of Product.
        public Shop shop { get; set; }
        public Guid ShopId { get; set; }


    }
}
