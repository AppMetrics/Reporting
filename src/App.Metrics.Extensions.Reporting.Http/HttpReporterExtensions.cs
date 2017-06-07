// <copyright file="HttpReporterExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Abstractions.Filtering;
using App.Metrics.Formatting.Ascii;
using App.Metrics.Reporting.Abstractions;

namespace App.Metrics.Extensions.Reporting.Http
{
    public static class HttpReporterExtensions
    {
        public static IReportFactory AddHttp(
            this IReportFactory factory,
            HttpReporterSettings settings,
            IFilterMetrics filter = null)
        {
            var payloadBuilder = new AsciiMetricPayloadBuilder(settings.MetricNameFormatter, settings.DataKeys);
            factory.AddProvider(new HttpReporterProvider<AsciiMetricPayload>(settings, payloadBuilder, filter));
            return factory;
        }

        public static IReportFactory AddHttp<TPayload>(
            this IReportFactory factory,
            HttpReporterSettings settings,
            IMetricPayloadBuilder<TPayload> payloadBuilder,
            IFilterMetrics filter = null)
        {
            factory.AddProvider(new HttpReporterProvider<TPayload>(settings, payloadBuilder, filter));
            return factory;
        }

        public static IReportFactory AddHttp(
            this IReportFactory factory,
            IFilterMetrics filter = null)
        {
            var settings = new HttpReporterSettings();
            factory.AddHttp(settings, filter);
            return factory;
        }
    }
}