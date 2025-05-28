namespace ShopOwnerCore.Application_Core.Enum
{
    public static class AppStatusCode
    {
        public static readonly int BadRequest = 400;
        public static readonly int Forbid = 403;
        public static readonly int NotFound = 404;
        public static readonly int EmailNotFound = 900;
        public static readonly int ResetPassWordTokenExpire = 901;
        public static readonly int EmailRegistered = 902;
    }
}
