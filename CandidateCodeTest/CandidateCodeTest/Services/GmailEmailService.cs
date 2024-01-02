using System;
using System.Net;
using System.Net.Mail;
using CandidateCodeTest.Contracts;

namespace CandidateCodeTest.Services
{
    public class GmailEmailService : IEmailService
    {
        private readonly string _senderEmail;
        private readonly string _password;

        public GmailEmailService(string senderEmail, string password)
        {
            _senderEmail = senderEmail;
            _password = password;
        }

        public void SendEmail(string to, string subject, string body)
        {
            try
            {
                using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(_senderEmail, _password);

                    using (var message = new MailMessage(_senderEmail, to))
                    {
                        message.Subject = subject;
                        message.Body = body;
                        message.IsBodyHtml = false;

                        smtpClient.Send(message);
                    }
                }
            }
            catch (SmtpException smtpEx)
            {
                // Handle SMTP exceptions here, such as authentication failed, or cannot connect
                Console.WriteLine($"SMTP Error: {smtpEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                // Handle any other general exceptions here
                Console.WriteLine($"An error occurred when sending the email: {ex.Message}");
                throw;
            }
        }
    }
}
