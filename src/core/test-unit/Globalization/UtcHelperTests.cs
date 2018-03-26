using System;
using RapidCore.Globalization;
using Xunit;

namespace RapidCore.UnitTests.Globalization
{
    public class UtcHelperTests
    {
        private readonly UtcHelper utcHelper;

        public UtcHelperTests()
        {
            utcHelper = new UtcHelper();
        }

        #region Helpers
        private TimeZoneInfo GetNonUtcTimeZone()
        {
            foreach (var tzi in TimeZoneInfo.GetSystemTimeZones())
            {
                if (tzi.BaseUtcOffset != TimeSpan.Zero)
                {
                    return tzi;
                }
            }
            
            throw new InvalidOperationException("Could not find a non-utc timezone on this system");
        }
        #endregion

        #region ToUtc - datetime input
        [Fact]
        public void ToUtc_datetime_alreadyUtc()
        {
            var given = DateTime.UtcNow;
            var actual = utcHelper.ToUtc(given);
            
            Assert.Equal(given, actual);
        }
        
        [Fact]
        public void ToUtc_datetime_nonUtc_is_converted()
        {
            /**
             * We need to create a DateTime that is not already UTC, but without
             * knowing what timezone the system running the code is set to.
             */
            var date = new DateTime(2018, 3, 21, 12, 13, 14, 666, DateTimeKind.Local);
            var input = TimeZoneInfo.ConvertTime(date, GetNonUtcTimeZone());
            
            var actual = utcHelper.ToUtc(input);
            
            Assert.Equal(input.ToUniversalTime(), actual);
        }
        #endregion
    }
}