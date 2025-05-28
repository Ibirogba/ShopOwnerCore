using Microsoft.AspNetCore.Identity;

namespace ShopOwnerCore.Application_Core.Entities.Models
{
    public class User:IdentityUser
    {
        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }



    }
}
