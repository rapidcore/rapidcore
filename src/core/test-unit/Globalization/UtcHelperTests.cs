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
        
        [Fact]
        public void ToUtc_datetime_unspecifiedKind_doesNotBlowUp()
        {
            var input = new DateTime(2018, 3, 21, 12, 13, 14, 666, DateTimeKind.Unspecified);
            
            var actual = utcHelper.ToUtc(input);
            
            Assert.Equal(input.ToUniversalTime(), actual);
        }
        #endregion

        #region ToUtc - string input
        [Theory]
        // already UTC
        [InlineData("2018-03-26Z", 2018, 3, 26, 0, 0, 0, 0)] // date + timezone
        [InlineData("2018-03-26T10:20+00:00", 2018, 3, 26, 10, 20, 0, 0)] // date, hour, minute, timezone
        [InlineData("2018-03-26T10:20:33+00:00", 2018, 3, 26, 10, 20, 33, 0)] // date, hour, minute, second, timezone
        [InlineData("2018-03-26T10:20:33.123+00:00", 2018, 3, 26, 10, 20, 33, 123)] // date, hour, minute, second, millisecond, timezone
        [InlineData("2018-03-26T10:20Z", 2018, 3, 26, 10, 20, 0, 0)] // date, hour, minute, Z
        [InlineData("2018-03-26T10:20:33Z", 2018, 3, 26, 10, 20, 33, 0)] // date, hour, minute, second, Z
        [InlineData("2018-03-26T10:20:33.123Z", 2018, 3, 26, 10, 20, 33, 123)] // date, hour, minute, second, millisecond, Z
        // non-UTC
        [InlineData("2018-03-26+04:00", 2018, 3, 25, 20, 0, 0, 0)] // date only, already utc
        [InlineData("2018-03-26T10:20+03:00", 2018, 3, 26, 7, 20, 0, 0)] // date, hour, minute, timezone
        [InlineData("2018-03-26T10:20:33+02:00", 2018, 3, 26, 8, 20, 33, 0)] // date, hour, minute, second, timezone
        [InlineData("2018-03-26T10:20:33.123+01:00", 2018, 3, 26, 9, 20, 33, 123)] // date, hour, minute, second, millisecond, timezone
        public void ToUtc_string_(string input, int expYear, int expMonth, int expDay, int expHour, int expMinute, int expSecond, int expMilli)
        {
            var actual = utcHelper.ToUtc(input);
            var expected = new DateTime(expYear, expMonth, expDay, expHour, expMinute, expSecond, expMilli, DateTimeKind.Utc);
            
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}