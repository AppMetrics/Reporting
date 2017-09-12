// <copyright file="IMetricsReportingBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

namespace App.Metrics.Reporting.Builder
{
    public interface IMetricsReportingBuilder
    {
        IMetricsRoot Metrics { get; }

        /// <summary>
        ///     Builder for configuring App Metrics Reporting options.
        /// </summary>
        MetricsReportingOptionsBuilder Options { get; }

        /// <summary>
        ///     <para>
        ///         Builder for configuring metrics reporter providers.
        ///     </para>
        /// </summary>
        MetricsReporterProviderBuilder ReportMetrics { get; }

        /// <summary>
        ///     Builds an <see cref="IMetricsReporter" /> with the services configured via an <see cref="IMetricsReportingBuilder" />.
        /// </summary>
        /// <returns>An <see cref="IMetricsReporter" /> with services configured via an <see cref="IMetricsReportingBuilder" />.</returns>
        IMetricsReporter Build();
    }
}