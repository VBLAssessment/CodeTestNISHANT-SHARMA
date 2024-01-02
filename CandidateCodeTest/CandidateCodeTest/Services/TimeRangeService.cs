using System;
using CandidateCodeTest.Contracts;

namespace CandidateCodeTest.Services
{
    public class TimeRangeService : ITimeRangeService
    {
        private readonly ITimeRangeService _timeProvider;

        public TimeRangeService(ITimeRangeService timeProvider)
        {
            _timeProvider = timeProvider;
        }

        public bool IsWithinTimeRange(TimeSpan start, TimeSpan end)
        {
            return _timeProvider.IsWithinTimeRange(start, end);
        }
    }
}
