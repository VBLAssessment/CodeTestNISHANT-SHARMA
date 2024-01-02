using System;

namespace CandidateCodeTest.Contracts
{
    public interface ITimeRangeService
    {
        bool IsWithinTimeRange(TimeSpan start, TimeSpan end);
    }


}
