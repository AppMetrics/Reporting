// <copyright file="TextFileReporterExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Abstractions.Filtering;
using App.Metrics.Formatting.Ascii;
using App.Metrics.Reporting.Abstractions;

namespace App.Metrics.Extensions.Reporting.TextFile
{
    public static class TextFileReporterExtensions
    {
        public static IReportFactory AddTextFile(
            this IReportFactory factory,
            TextFileReporterSettings settings,
            IFilterMetrics filter = null)
        {
            var payloadBuilder = new AsciiMetricPayloadBuilder(settings.MetricNameFormatter, settings.DataKeys);
            factory.AddProvider(new TextFileReporterProvider<AsciiMetricPayload>(settings, payloadBuilder, filter));
            return factory;
        }

        public static IReportFactory AddConsole<TPayload>(
            this IReportFactory factory,
            TextFileReporterSettings settings,
            IMetricPayloadBuilder<TPayload> payloadBuilder,
            IFilterMetrics filter = null)
        {
            factory.AddProvider(new TextFileReporterProvider<TPayload>(settings, payloadBuilder, filter));
            return factory;
        }

        public static IReportFactory AddTextFile(this IReportFactory factory, IFilterMetrics filter = null)
        {
            var settings = new TextFileReporterSettings();
            factory.AddTextFile(settings, filter);
            return factory;
        }
    }
}