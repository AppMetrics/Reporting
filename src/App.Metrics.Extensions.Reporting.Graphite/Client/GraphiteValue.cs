using System.Globalization;

namespace App.Metrics.Extensions.Reporting.Graphite.Client
{
    internal abstract class GraphiteValue
    {
        protected abstract string FormatString();

        /// <inheritdoc />
        public override string ToString() { return FormatString(); }
    }

    internal class GraphiteValue<T> : GraphiteValue
    {
        public T Value { get; set; }

        protected override string FormatString() => Value.ToString();
    }

    internal class GraphiteLongValue : GraphiteValue<long>
    {
        /// <inheritdoc />
        protected override string FormatString() => Value.ToString("D", CultureInfo.InvariantCulture);
    }

    internal class GraphiteDoubleValue : GraphiteValue<double>
    {
        /// <inheritdoc />
        protected override string FormatString() => Value.ToString("F", CultureInfo.InvariantCulture);
    }
}
