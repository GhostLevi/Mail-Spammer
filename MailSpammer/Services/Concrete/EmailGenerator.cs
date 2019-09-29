using MimeKit;
using Model;

namespace Services.Concrete
{
    public class EmailGenerator
    {
        private MimeMessage GenerateEmail(Person personData)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Rekiny", "rekinyprogramowania@gmail.com"));
            message.To.Add(new MailboxAddress($"{personData.FirstName} {personData.LastName}", personData.Email));

            message.Subject = $"New {personData.CarBrand} car deals!";

            var prefix = personData.Gender is Gender.Male ? "Mr." : "Ms.";

            message.Body = new TextPart("plain")
            {
                Text = $@"Dear {personData.FirstName} {personData.LastName},

Do you know that {personData.CarBrand} has new deals on 2019 car models?

Check it at http://www.{personData.CarBrand}.com

Best regards,
Sales Representative of {personData.CarBrand}"
            };

            return message;
        }
    }
}