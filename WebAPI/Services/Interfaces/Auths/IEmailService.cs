namespace WebAPI.Services.Interfaces.Auths
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
