using MimeKit;

namespace Services.Interface
{
    public interface IEmailService
    {
        MimeMessage PrepareEmail();
    }
}