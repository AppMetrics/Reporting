using App.Metrics.Extensions.Reporting.Graphite.Client;

namespace App.Metrics.Extensions.Reporting.Graphite.Extentions
{
    internal static class GraphiteValueExtentions
    {
        public static GraphiteValue ToGraphiteValue(this long value)
        {
            return new GraphiteLongValue { Value = value };
        }
        public static GraphiteValue ToGraphiteValue(this double value)
        {
            return new GraphiteDoubleValue { Value = value };
        }
        public static GraphiteValue ToGraphiteValue<T>(this T value)
        {
            return new GraphiteValue<T> { Value = value };
        }
    }
}