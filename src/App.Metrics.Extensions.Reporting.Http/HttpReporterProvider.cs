// <copyright file="HttpReporterProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Core.Filtering;
using App.Metrics.Extensions.Reporting.Http.Client;
using App.Metrics.Filters;
using App.Metrics.Reporting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace App.Metrics.Extensions.Reporting.Http
{
    public class HttpReporterProvider<TPayload> : IReporterProvider
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IMetricPayloadBuilder<TPayload> _payloadBuilder;
        private readonly HttpReporterSettings _settings;

        public HttpReporterProvider(
            HttpReporterSettings settings,
            ILoggerFactory loggerFactory,
            IMetricPayloadBuilder<TPayload> payloadBuilder,
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
            var httpClient = new DefaultHttpClient(
                _loggerFactory,
                _settings.HttpSettings,
                _settings.HttpPolicy,
                _settings.InnerHttpMessageHandler);

            return new ReportRunner<TPayload>(
                async p =>
                {
                    var result = await httpClient.WriteAsync(p.PayloadFormatted());
                    return result.Success;
                },
                _payloadBuilder,
                _settings.ReportInterval,
                name,
                _loggerFactory);
        }
    }
}