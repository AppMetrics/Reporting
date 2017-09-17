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
        }

        public ConsoleMetricsReporter(MetricsReportingConsoleOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.MetricsOutputFormatter != null)
            {
                Formatter = options.MetricsOutputFormatter;
            }

            if (options.FlushInterval > TimeSpan.Zero)
            {
                FlushInterval = options.FlushInterval;
            }

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
            var formatter = Formatter ?? _defaultMetricsOutputFormatter;

            using (var stream = new MemoryStream())
            {
                await formatter.WriteAsync(stream, metricsData, cancellationToken);

                var output = Encoding.UTF8.GetString(stream.ToArray());

                WriteLine(output);
            }

            return true;
        }
    }
}