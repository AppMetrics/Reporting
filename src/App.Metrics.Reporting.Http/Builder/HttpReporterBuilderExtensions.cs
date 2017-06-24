// <copyright file="HttpReporterBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Core.Filtering;
using App.Metrics.Filters;
using App.Metrics.Reporting;
using App.Metrics.Reporting.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

// ReSharper disable CheckNamespace
namespace App.Metrics.Builder
    // ReSharper restore CheckNamespace
{
    public static class HttpReporterBuilderExtensions
    {
        public static IReportFactory AddHttp<TPayload>(
            this IReportFactory factory,
            HttpReporterSettings settings,
            ILoggerFactory loggerFactory,
            IMetricPayloadBuilder<TPayload> payloadBuilder,
            IFilterMetrics filter = null)
        {
            filter = filter ?? new NoOpMetricsFilter();
            loggerFactory = loggerFactory ?? NullLoggerFactory.Instance;

            factory.AddProvider(new HttpReporterProvider<TPayload>(settings, loggerFactory, payloadBuilder, filter));

            return factory;
        }
    }
}