namespace ShopOwnerCore.Application_Core.Entities.Models
{
    public class Shop:BaseModel
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string? Region { get; set; }

        public string Suffix { get; set; }

        public string Country { get; set; }

       public ICollection<Product> Products { get; set; }

      public virtual User user { get; set; }

    }
}
