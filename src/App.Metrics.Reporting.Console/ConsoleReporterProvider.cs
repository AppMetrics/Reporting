// <copyright file="ConsoleReporterProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.Filtering;
using App.Metrics.Filters;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Reporting.Console
{
    public class ConsoleReporterProvider<TPayload> : IReporterProvider
    {
        private readonly IMetricPayloadBuilder<TPayload> _payloadBuilder;
        private readonly ConsoleReporterSettings _settings;

        public ConsoleReporterProvider(
            ConsoleReporterSettings settings,
            IMetricPayloadBuilder<TPayload> payloadBuilder)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _payloadBuilder = payloadBuilder ?? throw new ArgumentNullException(nameof(payloadBuilder));
            Filter = new NoOpMetricsFilter();
        }

        public ConsoleReporterProvider(
            ConsoleReporterSettings settings,
            IMetricPayloadBuilder<TPayload> payloadBuilder,
            IFilterMetrics filter)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _payloadBuilder = payloadBuilder ?? throw new ArgumentNullException(nameof(payloadBuilder));
            Filter = filter ?? new NoOpMetricsFilter();
        }

        public IFilterMetrics Filter { get; }

        public IMetricReporter CreateMetricReporter(string name, ILoggerFactory loggerFactory)
        {
            return new ReportRunner<TPayload>(
                p =>
                {
                    System.Console.WriteLine(p.PayloadFormatted());
                    return Task.FromResult(true);
                },
                _payloadBuilder,
                _settings.ReportInterval,
                name,
                loggerFactory);
        }
    }
}