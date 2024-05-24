namespace WebBanHang.Models
{
    public interface IEmailSender1
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
