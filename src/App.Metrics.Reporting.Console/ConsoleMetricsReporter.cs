// <copyright file="ConsoleMetricsReporter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Filters;
using App.Metrics.Formatters;
using App.Metrics.Formatters.Ascii;
using static System.Console;

namespace App.Metrics.Reporting.Console
{
    public class ConsoleMetricsReporter : IReportMetrics
    {
        private readonly IMetricsOutputFormatter _defaultMetricsOutputFormatter = new MetricsTextOutputFormatter();

        // ReSharper disable UnusedMember.Global
        public ConsoleMetricsReporter()
            // ReSharper restore UnusedMember.Global
        {
            FlushInterval = AppMetricsConstants.Reporting.DefaultFlushInterval;
            Formatter = _defaultMetricsOutputFormatter;
        }

        public ConsoleMetricsReporter(MetricsReportingConsoleOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.FlushInterval < TimeSpan.Zero)
            {
                throw new InvalidOperationException($"{nameof(MetricsReportingConsoleOptions.FlushInterval)} must not be less than zero");
            }

            Formatter = options.MetricsOutputFormatter ?? _defaultMetricsOutputFormatter;

            FlushInterval = options.FlushInterval > TimeSpan.Zero
                ? options.FlushInterval
                : AppMetricsConstants.Reporting.DefaultFlushInterval;

            Filter = options.Filter;
        }

        /// <inheritdoc />
        public IFilterMetrics Filter { get; set; }

        /// <inheritdoc />
        public TimeSpan FlushInterval { get; set; }

        /// <inheritdoc />
        public IMetricsOutputFormatter Formatter { get; set; }

        /// <inheritdoc />
        public async Task<bool> FlushAsync(MetricsDataValueSource metricsData, CancellationToken cancellationToken = default)
        {
            using (var stream = new MemoryStream())
            {
                await Formatter.WriteAsync(stream, metricsData, cancellationToken);

                var output = Encoding.UTF8.GetString(stream.ToArray());

                WriteLine(output);
            }

            return true;
        }
    }
}