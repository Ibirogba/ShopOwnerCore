namespace ShopOwnerCore.Application_Core.Enum
{
    public class JwtSetting
    {
        public string ValidIssuer { get; set; }

        public string ValidAudience { get; set; }

        public string Secret { get; set; }

        public int Expire { get; set; }
    }
}
