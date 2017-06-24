// <copyright file="ConsoleReporterProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.Core.Filtering;
using App.Metrics.Filters;
using App.Metrics.Reporting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace App.Metrics.Extensions.Reporting.Console
{
    public class ConsoleReporterProvider<TPayload> : IReporterProvider
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IMetricPayloadBuilder<TPayload> _payloadBuilder;
        private readonly ConsoleReporterSettings _settings;

        public ConsoleReporterProvider(
            ConsoleReporterSettings settings,
            IMetricPayloadBuilder<TPayload> payloadBuilder,
            ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory ?? NullLoggerFactory.Instance;
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _payloadBuilder = payloadBuilder ?? throw new ArgumentNullException(nameof(payloadBuilder));
            Filter = new NoOpMetricsFilter();
        }

        public ConsoleReporterProvider(
            ConsoleReporterSettings settings,
            IMetricPayloadBuilder<TPayload> payloadBuilder,
            ILoggerFactory loggerFactory,
            IFilterMetrics filter)
        {
            _loggerFactory = loggerFactory ?? NullLoggerFactory.Instance;
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _payloadBuilder = payloadBuilder ?? throw new ArgumentNullException(nameof(payloadBuilder));
            Filter = filter ?? new NoOpMetricsFilter();
        }

        public IFilterMetrics Filter { get; }

        public IMetricReporter CreateMetricReporter(string name)
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
                _loggerFactory);
        }
    }
}