using System;
using Microsoft.Extensions.Configuration;
using CandidateCodeTest.Services;
using CandidateCodeTest.Contracts;

public class Program
{
    public static IConfiguration Configuration { get; set; }

    static void Main(string[] args)
    {
        // Build a configuration
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        Configuration = builder.Build();

        // Retrieve settings
        string senderEmail = Configuration["EmailSettings:SenderEmail"];
        string password = Configuration["EmailSettings:Password"];

        IEmailService emailService = new GmailEmailService(senderEmail, password);
        ITimeRangeService timeRangeService = new TimeRangeService(new SystemTimeProvider());
        CustomerService customerService = new CustomerService(emailService, timeRangeService);

        // Attempt to send an email
        string recipient = "testreceiver@gmail.com";
        string subject = "Test Subject";
        string body = "This is a test email from the CustomerService application.";

        
        if (customerService.TrySendEmail(recipient, subject, body))
        {
            Console.WriteLine("Email sent successfully.");
        }
        else
        {
            Console.WriteLine("Email was not sent.");
        }
    }
}
