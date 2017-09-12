// <copyright file="MetricsConsoleReporterProviderBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Formatters.Ascii;
using App.Metrics.Reporting.Builder;
using App.Metrics.Reporting.Console;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Builder for configuring metrics console reporting using an
    ///     <see cref="IMetricsReportingBuilder" />.
    /// </summary>
    public static class MetricsConsoleReporterProviderBuilder
    {
        /// <summary>
        ///     Add the <see cref="ConsoleMetricsReporterProvider" /> allowing metrics to be reported to console.
        /// </summary>
        /// <param name="metricReporterProviderBuilder">
        ///     The <see cref="MetricsReporterProviderBuilder" /> used to configure metrics reporter providers.
        /// </param>
        /// <param name="setupAction">The console reporting options to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsReportingBuilder" /> that can be used to further configure App Metrics Reporting.
        /// </returns>
        public static IMetricsReportingBuilder ToConsole(
            this MetricsReporterProviderBuilder metricReporterProviderBuilder,
            Action<MetricsReportingConsoleOptions> setupAction = null)
        {
            if (metricReporterProviderBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricReporterProviderBuilder));
            }

            var options = new MetricsReportingConsoleOptions();

            setupAction?.Invoke(options);

            if (options.MetricsOutputFormatter == null)
            {
                options.MetricsOutputFormatter = metricReporterProviderBuilder.Metrics.OutputMetricsFormatters.GetType<MetricsTextOutputFormatter>();
            }

            var provider = new ConsoleMetricsReporterProvider(options);

            return metricReporterProviderBuilder.Using(provider);
        }
    }
}
