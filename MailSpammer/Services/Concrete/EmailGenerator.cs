using System.Net.Mail;
using System.Threading.Tasks;
using FluentEmail.Core;
using Microsoft.Extensions.Options;
using Model;
using Services.Interface;

namespace Services.Concrete
{
    public class EmailGenerator : IEmailGenerator
    {
        private readonly SmtpConfig _smtpConfig;

        public EmailGenerator(IOptions<SmtpConfig> smtpConfig)
        {
            _smtpConfig = smtpConfig.Value;
        }

        public Task<MailMessage> GenerateEmail(Person personData)
        {
            var subject = $"New {personData.CarBrand} car deals!";

            var body = PrepareBody(personData).Result;

            var mail = new MailMessage(_smtpConfig.Username, personData.Email, subject, body);

            return Task.FromResult(mail);
        }

        private Task<string> PrepareBody(Person personData)
        {
            var prefix = personData.Gender == Gender.Male ? "Mr." : "Ms.";

            var body = $@"Dear {prefix} {personData.FirstName} {personData.LastName},

Do you know that {personData.CarBrand} has new deals on 2019 car models?

Check it at http://www.{personData.CarBrand}.com

Best regards,
Sales Representative of {personData.CarBrand}";

            return Task.FromResult(body);
        }
    }
}