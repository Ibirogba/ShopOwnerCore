using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace ShopOwnerCore.Application_Core.Entities.View_Model
{
    public class ProductViewModel:BaseViewModel
    {
        
        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal OldUnitPrice { get; set; }

        public string PictureUrl { get; set; }

        public Guid ShopId { get; set; }


    }
}
