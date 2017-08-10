// <copyright file="ConsoleReporterProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Filters;
using Microsoft.Extensions.Options;

namespace App.Metrics.Reporting.Console
{
    public class ConsoleReporterProvider : IReporterProvider
    {
        private readonly IOptions<MetricsReportingConsoleOptions> _consoleOptionsAccessor;

        public ConsoleReporterProvider(
            IOptions<MetricsReportingOptions> optionsAccessor,
            IOptions<MetricsReportingConsoleOptions> consoleOptionsAccessor)
        {
            _consoleOptionsAccessor = consoleOptionsAccessor;
            Filter = optionsAccessor.Value.Filter;
            ReportInterval = consoleOptionsAccessor.Value.ReportInterval;
        }

        public IFilterMetrics Filter { get; }

        public TimeSpan ReportInterval { get; }

        public async Task<bool> FlushAsync(MetricsDataValueSource metricsData, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var stream = new MemoryStream())
            {
                await _consoleOptionsAccessor.Value.MetricsOutputFormatter.WriteAsync(stream, metricsData, cancellationToken);

                var output = Encoding.UTF8.GetString(stream.ToArray());

                System.Console.WriteLine(output);
            }

            return true;
        }
    }
}