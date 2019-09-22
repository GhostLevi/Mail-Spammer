using System;

namespace App
{
    public class SpamService : ISpamService
    {

        public void SendMails()
        {
            Console.WriteLine("Mails sent.");
        }
    }
}