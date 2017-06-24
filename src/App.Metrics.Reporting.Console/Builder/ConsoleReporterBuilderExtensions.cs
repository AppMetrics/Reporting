// <copyright file="ConsoleReporterBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Core.Filtering;
using App.Metrics.Filters;
using App.Metrics.Formatters.Ascii;
using App.Metrics.Reporting;
using App.Metrics.Reporting.Console;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

// ReSharper disable CheckNamespace
namespace App.Metrics.Builder
    // ReSharper restore CheckNamespace
{
    public static class ConsoleReporterBuilderExtensions
    {
        public static IReportFactory AddConsole(
            this IReportFactory factory,
            ConsoleReporterSettings settings,
            ILoggerFactory loggerFactory,
            IFilterMetrics filter = null)
        {
            filter = filter ?? new NoOpMetricsFilter();
            loggerFactory = loggerFactory ?? NullLoggerFactory.Instance;
            var payloadBuilder = new AsciiMetricPayloadBuilder(settings.MetricNameFormatter, settings.DataKeys);
            factory.AddProvider(new ConsoleReporterProvider<AsciiMetricPayload>(settings, payloadBuilder, loggerFactory, filter));
            return factory;
        }

        public static IReportFactory AddConsole<TPayload>(
            this IReportFactory factory,
            ConsoleReporterSettings settings,
            ILoggerFactory loggerFactory,
            IMetricPayloadBuilder<TPayload> payloadBuilder,
            IFilterMetrics filter = null)
        {
            filter = filter ?? new NoOpMetricsFilter();
            loggerFactory = loggerFactory ?? NullLoggerFactory.Instance;
            factory.AddProvider(new ConsoleReporterProvider<TPayload>(settings, payloadBuilder, loggerFactory, filter));
            return factory;
        }

        public static IReportFactory AddConsole(
            this IReportFactory factory,
            ILoggerFactory loggerFactory,
            IFilterMetrics filter = null)
        {
            filter = filter ?? new NoOpMetricsFilter();
            loggerFactory = loggerFactory ?? NullLoggerFactory.Instance;
            var settings = new ConsoleReporterSettings();
            factory.AddConsole(settings, loggerFactory, filter);
            return factory;
        }
    }
}