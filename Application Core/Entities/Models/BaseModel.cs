namespace ShopOwnerCore.Application_Core.Entities.Models
{
    public class BaseModel
    {
        public Guid id { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }
    }
}
