// <copyright file="ConsoleMetricsReporterProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Filters;

namespace App.Metrics.Reporting.Console
{
    public class ConsoleMetricsReporterProvider : IMetricsReporterProvider
    {
        private readonly MetricsReportingConsoleOptions _consoleReportingOptions;

        public ConsoleMetricsReporterProvider(MetricsReportingConsoleOptions consoleReportingOptions)
        {
            _consoleReportingOptions = consoleReportingOptions;
            Filter = consoleReportingOptions.Filter;
            ReportInterval = consoleReportingOptions.ReportInterval;
        }

        /// <inheritdoc />
        public IFilterMetrics Filter { get; }

        /// <inheritdoc />
        public TimeSpan ReportInterval { get; }

        /// <inheritdoc />
        public async Task<bool> FlushAsync(MetricsDataValueSource metricsData, CancellationToken cancellationToken = default)
        {
            using (var stream = new MemoryStream())
            {
                await _consoleReportingOptions.MetricsOutputFormatter.WriteAsync(stream, metricsData, cancellationToken);

                var output = Encoding.UTF8.GetString(stream.ToArray());

                System.Console.WriteLine(output);
            }

            return true;
        }
    }
}