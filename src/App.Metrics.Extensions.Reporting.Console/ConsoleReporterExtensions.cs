// <copyright file="ConsoleReporterExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Abstractions.Filtering;
using App.Metrics.Formatting.Ascii;
using App.Metrics.Reporting.Abstractions;

namespace App.Metrics.Extensions.Reporting.Console
{
    public static class ConsoleReporterExtensions
    {
        public static IReportFactory AddConsole(
            this IReportFactory factory,
            ConsoleReporterSettings settings,
            IFilterMetrics filter = null)
        {
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
            factory.AddProvider(new ConsoleReporterProvider<TPayload>(settings, payloadBuilder, filter));
            return factory;
        }

        public static IReportFactory AddConsole(
            this IReportFactory factory,
            IFilterMetrics filter = null)
        {
            var settings = new ConsoleReporterSettings();
            factory.AddConsole(settings, filter);
            return factory;
        }
    }
}