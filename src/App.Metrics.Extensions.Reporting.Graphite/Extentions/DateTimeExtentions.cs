using System;

namespace App.Metrics.Extensions.Reporting.Graphite.Extentions
{
    internal static class DateTimeExtentions
    {
        public static int ToUnixTime(this DateTime time) => (int)time.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
    }
}
