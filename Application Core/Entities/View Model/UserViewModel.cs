namespace ShopOwnerCore.Application_Core.Entities.View_Model
{
    public class UserViewModel
    {
        public required string UserName { get; set; }

        public required string Email { get; set; }

        public required string Password { get; set; }

        public List<string>? Roles { get; set; }

        public required string PhoneNumber { get; set; } 
    }
}
