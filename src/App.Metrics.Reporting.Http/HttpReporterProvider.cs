// <copyright file="HttpReporterProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Core.Filtering;
using App.Metrics.Filters;
using App.Metrics.Reporting.Http.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace App.Metrics.Reporting.Http
{
    public class HttpReporterProvider<TPayload> : IReporterProvider
    {
        private readonly IMetricPayloadBuilder<TPayload> _payloadBuilder;
        private readonly HttpReporterSettings _settings;

        public HttpReporterProvider(
            HttpReporterSettings settings,
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
            var httpClient = new DefaultHttpClient(
                loggerFactory,
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
                loggerFactory);
        }
    }
}