// <copyright file="ConsoleReporterProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.Abstractions.Filtering;
using App.Metrics.Abstractions.Reporting;
using App.Metrics.Reporting;
using App.Metrics.Reporting.Abstractions;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Extensions.Reporting.Console
{
    public class ConsoleReporterProvider<TPayload> : IReporterProvider
    {
        private readonly IMetricPayloadBuilder<TPayload> _payloadBuilder;
        private readonly ConsoleReporterSettings _settings;

        public ConsoleReporterProvider(ConsoleReporterSettings settings, IMetricPayloadBuilder<TPayload> payloadBuilder, IFilterMetrics filter)
        {
            _payloadBuilder = payloadBuilder;
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));

            Filter = filter;
        }

        public IFilterMetrics Filter { get; }

        public IMetricReporter CreateMetricReporter(string name, ILoggerFactory loggerFactory)
        {
            return new ReportRunner<TPayload>(
                p =>
                {
                    System.Console.WriteLine(p.PayloadFormatted());
                    return AppMetricsTaskCache.SuccessTask;
                },
                _payloadBuilder,
                _settings.ReportInterval,
                name,
                loggerFactory);
        }
    }
}