using Microsoft.AspNetCore.Identity;

namespace ShopOwnerCore.Application_Core.Entities.Models
{
    public class ApplicationRole:IdentityRole
    {
        public virtual ICollection<IdentityUserRole<string>> Users { get; set; }

        public virtual ICollection<IdentityRoleClaim<string>> Claims { get; set; }
    }
}
