// <copyright file="TextFileReporterBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Core.Filtering;
using App.Metrics.Extensions.Reporting.TextFile;
using App.Metrics.Filters;
using App.Metrics.Formatters.Ascii;
using App.Metrics.Reporting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

// ReSharper disable CheckNamespace
namespace App.Metrics.Builder
    // ReSharper restore CheckNamespace
{
    public static class TextFileReporterBuilderExtensions
    {
        public static IReportFactory AddTextFile(
            this IReportFactory factory,
            TextFileReporterSettings settings,
            ILoggerFactory loggerFactory,
            IFilterMetrics filter = null)
        {
            filter = filter ?? new NoOpMetricsFilter();
            loggerFactory = loggerFactory ?? NullLoggerFactory.Instance;
            var payloadBuilder = new AsciiMetricPayloadBuilder(settings.MetricNameFormatter, settings.DataKeys);
            factory.AddProvider(new TextFileReporterProvider<AsciiMetricPayload>(settings, loggerFactory, payloadBuilder, filter));
            return factory;
        }

        public static IReportFactory AddTextFile<TPayload>(
            this IReportFactory factory,
            TextFileReporterSettings settings,
            ILoggerFactory loggerFactory,
            IMetricPayloadBuilder<TPayload> payloadBuilder,
            IFilterMetrics filter = null)
        {
            filter = filter ?? new NoOpMetricsFilter();
            loggerFactory = loggerFactory ?? NullLoggerFactory.Instance;
            factory.AddProvider(new TextFileReporterProvider<TPayload>(settings, loggerFactory, payloadBuilder, filter));
            return factory;
        }

        public static IReportFactory AddTextFile(
            this IReportFactory factory,
            ILoggerFactory loggerFactory,
            IFilterMetrics filter = null)
        {
            filter = filter ?? new NoOpMetricsFilter();
            loggerFactory = loggerFactory ?? NullLoggerFactory.Instance;
            var settings = new TextFileReporterSettings();
            factory.AddTextFile(settings, loggerFactory, filter);
            return factory;
        }
    }
}