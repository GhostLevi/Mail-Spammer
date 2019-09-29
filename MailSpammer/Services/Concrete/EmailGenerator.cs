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

        public Task<IFluentEmail> GenerateEmail(Person personData)
        {
            var subject = $"New {personData.CarBrand} car deals!";

            var body = PrepareBody(personData).Result;

            var mail = Email.From(_smtpConfig.Username)
                .To(personData.Email)
                .Subject(subject)
                .Body(body);

            return Task.FromResult(mail);
        }

        private Task<string> PrepareBody(Person personData)
        {
            var body = $@"Dear {personData.FirstName} {personData.LastName},

Do you know that {personData.CarBrand} has new deals on 2019 car models?

Check it at http://www.{personData.CarBrand}.com

Best regards,
Sales Representative of {personData.CarBrand}";

            return Task.FromResult(body);
        }
    }
}