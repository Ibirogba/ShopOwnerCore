namespace ShopOwnerCore.Application_Core.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string receiver, string message, EmailOptions options, EmailType emailType);
    }
}
