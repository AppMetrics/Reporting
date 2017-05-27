// <copyright file="HttpReporterProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Abstractions.Filtering;
using App.Metrics.Abstractions.Reporting;
using App.Metrics.Extensions.Reporting.Http.Client;
using App.Metrics.Reporting;
using App.Metrics.Reporting.Abstractions;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Extensions.Reporting.Http
{
    public class HttpReporterProvider<TPayload> : IReporterProvider
    {
        private readonly IMetricPayloadBuilder<TPayload> _payloadBuilder;
        private readonly HttpReporterSettings _settings;

        public HttpReporterProvider(HttpReporterSettings settings, IMetricPayloadBuilder<TPayload> payloadBuilder, IFilterMetrics filter)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _payloadBuilder = payloadBuilder ?? throw new ArgumentNullException(nameof(payloadBuilder));
            Filter = filter;
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