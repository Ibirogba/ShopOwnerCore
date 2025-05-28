using System.ComponentModel.DataAnnotations;

namespace ShopOwnerCore.Application_Core.Entities.View_Model
{
    public class ShopViewModel:BaseViewModel
    {
        [Required(ErrorMessage = "Shop Name is a required field")]
        [MaxLength(60, ErrorMessage = "Maximum Length for the Name is 60")]
        public string Name { get; set; }

        [Required(ErrorMessage = " Address is a required field")]
        [MaxLength(40, ErrorMessage ="Maximum Length for the Address is 40 ")]
        public string Address { get; set; }

        [Required(ErrorMessage= " City is a required field ")]
        [MaxLength(20, ErrorMessage = " Maximum Length for City is 20 ")]
        public string City { get; set; }

        
        public string? Region { get; set; }

        [Required(ErrorMessage = " Suffix is a required field")]
        [MaxLength(5, ErrorMessage ="Maximum length for suffix is 5")]
        public string Suffix { get; set; }

        [Required(ErrorMessage =" Country is a required field")]
        [MaxLength(20, ErrorMessage ="Maximum length for Country is 20")]
        public string Country { get; set; }

        public UserViewModel User { get; set; }

        public IEnumerable<ProductViewModel> Products { get; set; }

    }
}
