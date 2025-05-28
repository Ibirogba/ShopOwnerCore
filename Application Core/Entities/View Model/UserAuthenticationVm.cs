using Microsoft.OpenApi.MicrosoftExtensions;
using System.ComponentModel.DataAnnotations;

namespace ShopOwnerCore.Application_Core.Entities.View_Model
{
    public class UserAuthenticationVm
    {
        [Required(ErrorMessage ="User name is required")]
        public string userName { get; set; }

        [Required(ErrorMessage ="Password name is required")]
        public string Password { get; set; }
    }
}
