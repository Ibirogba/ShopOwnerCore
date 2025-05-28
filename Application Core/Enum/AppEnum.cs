namespace ShopOwnerCore.Application_Core.Enum
{
    public class AppEnum
    {
        public static readonly int MaxFailedAccessAttempts = 2;
        public static readonly int MinPasswordChar = 8;
        
    }
    public static class UserClaimsKey
    {
        public static readonly string Sub = "sub";
        public static readonly string Name = "name";
        public static readonly string Role = "role";
    }
}
