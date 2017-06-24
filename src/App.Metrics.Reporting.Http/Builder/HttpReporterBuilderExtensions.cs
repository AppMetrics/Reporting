// <copyright file="HttpReporterBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Core.Filtering;
using App.Metrics.Filters;
using App.Metrics.Reporting;
using App.Metrics.Reporting.Http;

// ReSharper disable CheckNamespace
namespace App.Metrics.Builder
    // ReSharper restore CheckNamespace
{
    public static class HttpReporterBuilderExtensions
    {
        public static IReportFactory AddHttp<TPayload>(
            this IReportFactory factory,
            HttpReporterSettings settings,
            IMetricPayloadBuilder<TPayload> payloadBuilder,
            IFilterMetrics filter = null)
        {
            filter = filter ?? new NoOpMetricsFilter();

            factory.AddProvider(new HttpReporterProvider<TPayload>(settings,  payloadBuilder, filter));

            return factory;
        }
    }
}