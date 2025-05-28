namespace ShopOwnerCore.Application_Core.Entities.Models
{
    public class PasswordRequest:BaseModel
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public bool isActive { get; set; }

    }
}
