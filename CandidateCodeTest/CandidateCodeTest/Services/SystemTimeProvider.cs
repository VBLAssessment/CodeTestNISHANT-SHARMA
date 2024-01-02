using System;
using CandidateCodeTest.Contracts;

namespace CandidateCodeTest.Services
{
    public class SystemTimeProvider : ITimeRangeService
    {
        public bool IsWithinTimeRange(TimeSpan start, TimeSpan end)
        {
            var now = DateTime.Now.TimeOfDay;
            return now > start && now < end;
        }
    }
}
