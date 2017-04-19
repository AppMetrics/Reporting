using System.Collections.Generic;
using System.Linq;
using App.Metrics.Apdex;
using App.Metrics.Core.Abstractions;
using App.Metrics.Counter;
using App.Metrics.Extensions.Reporting.Graphite.Client;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer;

namespace App.Metrics.Extensions.Reporting.Graphite.Extentions
{
    internal static class MetricValueSourceExtentions
    {
        public static GraphiteMetricName ToGraphiteName<T>(this MetricValueSourceBase<T> valueSource, MetricType type) => GraphiteMetricName.From(type, valueSource.Unit).WithFolder(valueSource.Name);

        public static IEnumerable<(GraphiteMetricName name, GraphiteValue value)> GetCounterItemsToSend(this CounterValueSource valueSource)
        {
            var name = valueSource.ToGraphiteName(MetricType.Counter);

            yield return (valueSource.Value.Items.Any() ? name.WithFolder("Total") : name, valueSource.Value.Count.ToGraphiteValue());

            if (!valueSource.ReportSetItems)
            {
                yield break;
            }

            foreach (var item in valueSource.Value.Items)
            {
                yield return (name.WithFolder(item.Item), item.Count.ToGraphiteValue());

                if (valueSource.ReportItemPercentages)
                {
                    yield return (name.WithFolder(item.Item, "Percent"), item.Percent.ToGraphiteValue());
                }
            }
        }

        public static IEnumerable<(GraphiteMetricName name, GraphiteValue value)> GetHistogramItemsToSend(this MetricValueSourceBase<HistogramValue> valueSource)
        {
            var name = valueSource.ToGraphiteName(MetricType.Histogram);
            var value = valueSource.Value;

            yield return (name.WithFolder("Count"), value.Count.ToGraphiteValue());
            yield return (name.WithFolder("Last"), value.LastValue.ToGraphiteValue());
            yield return (name.WithFolder("Min"), value.Min.ToGraphiteValue());
            yield return (name.WithFolder("Mean"), value.Mean.ToGraphiteValue());
            yield return (name.WithFolder("Max"), value.Max.ToGraphiteValue());
            yield return (name.WithFolder("StdDev"), value.StdDev.ToGraphiteValue());
            yield return (name.WithFolder("Median"), value.Median.ToGraphiteValue());
            yield return (name.WithFolder("p75"), value.Percentile75.ToGraphiteValue());
            yield return (name.WithFolder("p95"), value.Percentile95.ToGraphiteValue());
            yield return (name.WithFolder("p98"), value.Percentile98.ToGraphiteValue());
            yield return (name.WithFolder("p99"), value.Percentile99.ToGraphiteValue());
            yield return (name.WithFolder("p99,9"), value.Percentile999.ToGraphiteValue());
        }

        public static IEnumerable<(GraphiteMetricName name, GraphiteValue value)> GetMeterItemsToSend(this MetricValueSourceBase<MeterValue> valueSource)
        {
            var name = valueSource.ToGraphiteName(MetricType.Meter);
            var value = valueSource.Value;

            yield return (name.WithFolder("Total"), value.Count.ToGraphiteValue());

            foreach (var itemToSend in value.GetMeterItemsToSend(name))
            {
                yield return itemToSend;
            }

            foreach (var item in value.Items)
            {
                yield return (name.WithFolder(item.Item, "Count"), item.Value.Count.ToGraphiteValue());
                yield return (name.WithFolder(item.Item, "Percent"), item.Percent.ToGraphiteValue());

                foreach (var itemToSend in item.Value.GetMeterItemsToSend(name))
                {
                    yield return itemToSend;
                }
            }
        }

        public static IEnumerable<(GraphiteMetricName name, GraphiteValue value)> GetTimerItemsToSend(this TimerValueSource valueSource)
        {
            var name = valueSource.ToGraphiteName(MetricType.Timer);
            var value = valueSource.Value;

            var durationUnit = valueSource.DurationUnit;

            yield return (name.WithFolder("Count"), value.Rate.Count.ToGraphiteValue());
            yield return (name.WithFolder("Active_Sessions"), value.ActiveSessions.ToGraphiteValue());

            foreach (var item in value.Rate.GetMeterItemsToSend(name))
            {
                yield return item;
            }

            name = name.WithUnit(durationUnit.Unit());

            yield return (name.WithFolder("Duration-Last"), value.Histogram.LastValue.ToGraphiteValue());
            yield return (name.WithFolder("Duration-Min"), value.Histogram.Min.ToGraphiteValue());
            yield return (name.WithFolder("Duration-Mean"), value.Histogram.Mean.ToGraphiteValue());
            yield return (name.WithFolder("Duration-Max"), value.Histogram.Max.ToGraphiteValue());
            yield return (name.WithFolder("Duration-StdDev"), value.Histogram.StdDev.ToGraphiteValue());
            yield return (name.WithFolder("Duration-Median"), value.Histogram.Median.ToGraphiteValue());
            yield return (name.WithFolder("Duration-p75"), value.Histogram.Percentile75.ToGraphiteValue());
            yield return (name.WithFolder("Duration-p95"), value.Histogram.Percentile95.ToGraphiteValue());
            yield return (name.WithFolder("Duration-p98"), value.Histogram.Percentile98.ToGraphiteValue());
            yield return (name.WithFolder("Duration-p99"), value.Histogram.Percentile99.ToGraphiteValue());
            yield return (name.WithFolder("Duration-p999"), value.Histogram.Percentile999.ToGraphiteValue());
        }

        public static IEnumerable<(GraphiteMetricName name, GraphiteValue value)> GetGaugeItemsToSend(this MetricValueSourceBase<double> valueSource)
        {
            if (!double.IsNaN(valueSource.Value) && !double.IsInfinity(valueSource.Value))
            {
                yield return (valueSource.ToGraphiteName(MetricType.Gauge), valueSource.Value.ToGraphiteValue());
            }
        }

        public static IEnumerable<(GraphiteMetricName name, GraphiteValue value)> GetApdexItemsToSend(this MetricValueSourceBase<ApdexValue> valueSource)
        {
            var name = valueSource.ToGraphiteName(MetricType.Apdex);
            var value = valueSource.Value;

            yield return (name.WithFolder("Samples"), value.SampleSize.ToGraphiteValue());

            if (!double.IsNaN(value.Score) && !double.IsInfinity(value.Score))
            {
                yield return (name.WithFolder("Score"), value.Score.ToGraphiteValue());
            }

            yield return (name.WithFolder("Satisfied"), value.Satisfied.ToGraphiteValue());
            yield return (name.WithFolder("Tolerating"), value.Tolerating.ToGraphiteValue());
            yield return (name.WithFolder("Frustrating"), value.Frustrating.ToGraphiteValue());
        }

        private static IEnumerable<(GraphiteMetricName name, GraphiteValue value)> GetMeterItemsToSend(this MeterValue value, GraphiteMetricName name)
        {
            var rateUnit = value.RateUnit;
            name = name.WithRate(rateUnit);
            yield return (name.WithFolder("Rate-Mean"), value.MeanRate.ToGraphiteValue());
            yield return (name.WithFolder("Rate-1-min"), value.OneMinuteRate.ToGraphiteValue());
            yield return (name.WithFolder("Rate-5-min"), value.FiveMinuteRate.ToGraphiteValue());
            yield return (name.WithFolder("Rate-15-min"), value.FifteenMinuteRate.ToGraphiteValue());
        }
    }
}
