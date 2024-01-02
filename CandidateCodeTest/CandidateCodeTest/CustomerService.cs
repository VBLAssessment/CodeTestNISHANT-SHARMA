using CandidateCodeTest.Contracts;
using System;

public class CustomerService
{
    private readonly IEmailService _emailService;
    private readonly ITimeRangeService _timeRangeService;

    public CustomerService(IEmailService emailService, ITimeRangeService timeRangeService)
    {
        _emailService = emailService;
        _timeRangeService = timeRangeService;
    }

    public bool TrySendEmail(string recipient, string subject, string message, bool checkTimeRange = false)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(recipient);
            if (addr.Address != recipient)
            {
                Console.WriteLine("Invalid email address format.");
                return false;
            }
        }
        catch
        {
            Console.WriteLine("Invalid email address format.");
            return false;
        }

        if (checkTimeRange)
        {
            TimeSpan start = new TimeSpan(10, 0, 0); // start time
            TimeSpan end = new TimeSpan(12, 0, 0); // end time

            if (!_timeRangeService.IsWithinTimeRange(start, end))
            {
                Console.WriteLine("It's not the right time to send an email.");
                return false;
            }
        }

        _emailService.SendEmail(recipient, subject, message);
        return true;
    }
}


