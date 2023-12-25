using System.Globalization;

namespace Extensions.Pack
{
    public static class DateTimeExtensions
    {
        public static DateTime ChangeTime(this DateTime dateTime, int hours = 0, int minutes = 0, int seconds = 0)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hours, minutes, seconds);
        }

        public static DateTime ChangeHours(this DateTime dateTime, int hours)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hours, dateTime.Minute, dateTime.Second);
        }

        public static DateTime ChangeMinutes(this DateTime dateTime, int minutes)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, minutes, dateTime.Second);
        }

        public static int HoursPerDay(this DateTime dateTime)
        {
            var designators = CultureInfo.CurrentCulture.GetDesignators();
            var shortDateTime = dateTime.ToShortTimeString();
            return designators.Any(item => shortDateTime.Contains(item)) ? 12 : 24;
        }

        public static bool IsInternationalTime(this DateTime dateTime)
        {
            var result = dateTime.HoursPerDay();
            return result == 12;
        }

        public static string? ActiveDesignator(this DateTime dateTime)
        {
            var designators = CultureInfo.CurrentCulture.GetDesignators();
            var shortDateTime = dateTime.ToShortTimeString();

            return designators.FirstOrDefault(item => shortDateTime.Contains(item));
        }

        public static TimeSpan ToTimeSpan(this DateTime dateTime)
        {
            return new TimeSpan(0, dateTime.Hour, dateTime.Minute, dateTime.Second);
        }

        public static bool IntersectsWith(this DateTime dateTime, DateTime startTime, DateTime endTime)
        {
            var masterTimeRanges = CreateTimeRanges(startTime, endTime);
            var slaveTimeRanges = CreateTimeRanges(dateTime);
            return masterTimeRanges.Any(master => slaveTimeRanges.Any(slave => master.Intersect(slave).Any()));
        }

        public static IEnumerable<IEnumerable<int>> CreateTimeRanges(DateTime startDateTime, DateTime endDateTime)
        {
            const int minutesPerDay = 24 * 60;
            var timeSpanStart = startDateTime.ToTimeSpan();
            var timeSpanEnd = endDateTime.ToTimeSpan();

            var result = timeSpanEnd.TotalMinutes - timeSpanStart.TotalMinutes;
            if (result < 0)
            {
                yield return Enumerable.Range(timeSpanStart.TotalMinutes.Cast<int>(), minutesPerDay - timeSpanStart.TotalMinutes.Cast<int>());
                yield return Enumerable.Range(0, timeSpanEnd.TotalMinutes.Cast<int>());
                yield break;
            }

            var diffMinutes = timeSpanEnd.TotalMinutes - timeSpanStart.TotalMinutes;
            yield return Enumerable.Range(timeSpanStart.TotalMinutes.Cast<int>(), diffMinutes.Cast<int>());
        }

        private static IEnumerable<IEnumerable<int>> CreateTimeRanges(DateTime startDateTime)
        {
            var timeSpanStart = startDateTime.ToTimeSpan();
            yield return Enumerable.Range(timeSpanStart.TotalMinutes.Cast<int>(), 1);
        }
    }
}
