using System.Net.Mail;
using System.Threading.Tasks;
using FluentEmail.Core;
using Model;

namespace Services.Interface
{
    public interface IEmailGenerator
    {
        Task<MailMessage> GenerateEmail(Person personData);
    }
}