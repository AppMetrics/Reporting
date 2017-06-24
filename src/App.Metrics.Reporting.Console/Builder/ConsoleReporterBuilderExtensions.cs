// <copyright file="ConsoleReporterBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Core.Filtering;
using App.Metrics.Filters;
using App.Metrics.Formatters.Ascii;
using App.Metrics.Reporting;
using App.Metrics.Reporting.Console;

// ReSharper disable CheckNamespace
namespace App.Metrics.Builder
    // ReSharper restore CheckNamespace
{
    public static class ConsoleReporterBuilderExtensions
    {
        public static IReportFactory AddConsole(
            this IReportFactory factory,
            ConsoleReporterSettings settings,
            IFilterMetrics filter = null)
        {
            filter = filter ?? new NoOpMetricsFilter();
            var payloadBuilder = new AsciiMetricPayloadBuilder(settings.MetricNameFormatter, settings.DataKeys);
            factory.AddProvider(new ConsoleReporterProvider<AsciiMetricPayload>(settings, payloadBuilder, filter));
            return factory;
        }

        public static IReportFactory AddConsole<TPayload>(
            this IReportFactory factory,
            ConsoleReporterSettings settings,
            IMetricPayloadBuilder<TPayload> payloadBuilder,
            IFilterMetrics filter = null)
        {
            filter = filter ?? new NoOpMetricsFilter();
            factory.AddProvider(new ConsoleReporterProvider<TPayload>(settings, payloadBuilder, filter));
            return factory;
        }

        public static IReportFactory AddConsole(
            this IReportFactory factory,
            IFilterMetrics filter = null)
        {
            filter = filter ?? new NoOpMetricsFilter();
            var settings = new ConsoleReporterSettings();
            factory.AddConsole(settings, filter);
            return factory;
        }
    }
}