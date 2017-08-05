// <copyright file="TextFileReporterBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Filtering;
using App.Metrics.Filters;
using App.Metrics.Formatters.Ascii;
using App.Metrics.Reporting;
using App.Metrics.Reporting.TextFile;

// ReSharper disable CheckNamespace
namespace App.Metrics.Builder
    // ReSharper restore CheckNamespace
{
    public static class TextFileReporterBuilderExtensions
    {
        public static IReportFactory AddTextFile(
            this IReportFactory factory,
            TextFileReporterSettings settings,
            IFilterMetrics filter = null)
        {
            filter = filter ?? new NoOpMetricsFilter();
            var payloadBuilder = new AsciiMetricPayloadBuilder(settings.MetricNameFormatter, settings.DataKeys);
            factory.AddProvider(new TextFileReporterProvider<AsciiMetricPayload>(settings, payloadBuilder, filter));
            return factory;
        }

        public static IReportFactory AddTextFile<TPayload>(
            this IReportFactory factory,
            TextFileReporterSettings settings,
            IMetricPayloadBuilder<TPayload> payloadBuilder,
            IFilterMetrics filter = null)
        {
            filter = filter ?? new NoOpMetricsFilter();
            factory.AddProvider(new TextFileReporterProvider<TPayload>(settings, payloadBuilder, filter));
            return factory;
        }

        public static IReportFactory AddTextFile(
            this IReportFactory factory,
            IFilterMetrics filter = null)
        {
            filter = filter ?? new NoOpMetricsFilter();
            var settings = new TextFileReporterSettings();
            factory.AddTextFile(settings, filter);
            return factory;
        }
    }
}