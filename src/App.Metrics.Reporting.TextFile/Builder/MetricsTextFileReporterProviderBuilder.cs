// <copyright file="MetricsTextFileReporterProviderBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Formatters.Ascii;
using App.Metrics.Reporting.Builder;
using App.Metrics.Reporting.TextFile;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Builder for configuring metrics text file reporting using an
    ///     <see cref="IMetricsReportingBuilder" />.
    /// </summary>
    public static class MetricsTextFileReporterProviderBuilder
    {
        /// <summary>
        ///     Add the <see cref="TextFileMetricsReporterProvider" /> allowing metrics to be reported to text file.
        /// </summary>
        /// <param name="metricReporterProviderBuilder">
        ///     The <see cref="MetricsReporterProviderBuilder" /> used to configure metrics reporter providers.
        /// </param>
        /// <param name="setupAction">The text file reporting options to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsReportingBuilder" /> that can be used to further configure App Metrics Reporting.
        /// </returns>
        public static IMetricsReportingBuilder ToTextFile(
            this MetricsReporterProviderBuilder metricReporterProviderBuilder,
            Action<MetricsReportingTextFileOptions> setupAction = null)
        {
            if (metricReporterProviderBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricReporterProviderBuilder));
            }

            var options = new MetricsReportingTextFileOptions();

            setupAction?.Invoke(options);

            if (options.MetricsOutputFormatter == null)
            {
                options.MetricsOutputFormatter = metricReporterProviderBuilder.Metrics.OutputMetricsFormatters.GetType<MetricsTextOutputFormatter>();
            }

            var provider = new TextFileMetricsReporterProvider(options);

            return metricReporterProviderBuilder.Using(provider);
        }
    }
}
