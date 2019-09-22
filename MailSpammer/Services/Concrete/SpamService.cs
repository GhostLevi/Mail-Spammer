using System;
using Services.Interface;

namespace Services.Concrete
{
    public class SpamService : ISpamService
    {

        public void SendMails()
        {
            Console.WriteLine("Mails sent.");
        }
    }
}