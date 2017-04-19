using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Metrics.Abstractions.Reporting;
using App.Metrics.Apdex;
using App.Metrics.Core.Abstractions;
using App.Metrics.Counter;
using App.Metrics.Extensions.Reporting.Graphite.Client;
using App.Metrics.Extensions.Reporting.Graphite.Extentions;
using App.Metrics.Health;
using App.Metrics.Histogram;
using App.Metrics.Infrastructure;
using App.Metrics.Meter;
using App.Metrics.Tagging;
using App.Metrics.Timer;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Extensions.Reporting.Graphite
{
    public class GraphiteReporter : IMetricReporter
    {
        private readonly IGraphiteNameFormatter _nameFormatter;
        private readonly GraphiteSender _graphiteSender;
        private readonly ILogger<GraphiteReporter> _logger;

        public GraphiteReporter(GraphiteSender graphiteSender, IGraphiteNameFormatter nameFormatter, string name, TimeSpan reportInterval, ILoggerFactory loggerFactory)
        {
            _graphiteSender = graphiteSender;
            _nameFormatter = nameFormatter;
            _logger = loggerFactory.CreateLogger< GraphiteReporter>();

            Name = name;
            ReportInterval = reportInterval;
        }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public TimeSpan ReportInterval { get; }

        /// <inheritdoc />
        public void Dispose() => _graphiteSender.Dispose(); 

        /// <inheritdoc />
        public Task<bool> EndAndFlushReportRunAsync(IMetrics metrics) => _graphiteSender.Flush().ContinueWith(t => t.IsCompleted);

        /// <inheritdoc />
        public void StartReportRun(IMetrics metrics) {  }

        /// <inheritdoc />
        public void ReportEnvironment(EnvironmentInfo environmentInfo) { }

        /// <inheritdoc />
        public void ReportHealth(
            GlobalMetricTags globalTags,
            IEnumerable<HealthCheck.Result> healthyChecks,
            IEnumerable<HealthCheck.Result> degradedChecks,
            IEnumerable<HealthCheck.Result> unhealthyChecks)
        {
            // Health checks are reported as metrics as well
        }

        /// <inheritdoc />
        public void ReportMetric<T>(string context, MetricValueSourceBase<T> valueSource)
        {
            _logger.LogTrace($"Sending Metric {typeof(T)} for {Name}");

            if (typeof(T) == typeof(double))
            {
                ReportGauge(context, valueSource as MetricValueSourceBase<double>);
                return;
            }

            if (typeof(T) == typeof(CounterValue))
            {
                ReportCounter(context, valueSource as MetricValueSourceBase<CounterValue>);
                return;
            }

            if (typeof(T) == typeof(MeterValue))
            {
                ReportMeter(context, valueSource as MetricValueSourceBase<MeterValue>);
                return;
            }

            if (typeof(T) == typeof(TimerValue))
            {
                ReportTimer(context, valueSource as MetricValueSourceBase<TimerValue>);
                return;
            }

            if (typeof(T) == typeof(HistogramValue))
            {
                ReportHistogram(context, valueSource as MetricValueSourceBase<HistogramValue>);
                return;
            }

            if (typeof(T) == typeof(ApdexValue))
            {
                ReportApdex(context, valueSource as MetricValueSourceBase<ApdexValue>);
                return;
            }

            _logger.LogTrace($"Metric {typeof(T)} for {Name} was not sent");

        }

        private void ReportApdex(string context, MetricValueSourceBase<ApdexValue> valueSource)
        {
            foreach (var item in valueSource.GetApdexItemsToSend())
            {
                Send(context, valueSource.Tags, item.name, item.value);
            }
        }

        private void ReportCounter(string context, MetricValueSourceBase<CounterValue> valueSource)
        {
            var counterValueSource = valueSource as CounterValueSource;

            if (counterValueSource == null)
            {
                return;
            }

            foreach (var item in counterValueSource.GetCounterItemsToSend())
            {
                Send(context, valueSource.Tags, item.name, item.value);
            }
        }

        private void ReportGauge(string context, MetricValueSourceBase<double> valueSource)
        {
            foreach (var item in valueSource.GetGaugeItemsToSend())
            {
                Send(context, valueSource.Tags, item.name, item.value);
            }
        }

        private void ReportHistogram(string context, MetricValueSourceBase<HistogramValue> valueSource)
        {
            foreach (var item in valueSource.GetHistogramItemsToSend())
            {
                Send(context, valueSource.Tags, item.name, item.value);
            }
        }

        private void ReportMeter(string context, MetricValueSourceBase<MeterValue> valueSource)
        {
            foreach (var item in valueSource.GetMeterItemsToSend())
            {
                Send(context, valueSource.Tags, item.name, item.value);
            }
        }

        private void ReportTimer(string context, MetricValueSourceBase<TimerValue> valueSource)
        {
            var timerValueSource = valueSource as TimerValueSource;

            if (timerValueSource == null)
            {
                return;
            }

            foreach (var item in timerValueSource.GetTimerItemsToSend())
            {
                Send(context, valueSource.Tags, item.name, item.value);
            }
        }
        
        private void Send(string context, MetricTags tags, GraphiteName name, GraphiteValue value)
        {
            _graphiteSender.Send(_nameFormatter.Format(context, tags, name), value.ToString());
        }
    }
}