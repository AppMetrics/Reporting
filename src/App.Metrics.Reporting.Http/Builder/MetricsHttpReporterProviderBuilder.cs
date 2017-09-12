// <copyright file="MetricsHttpReporterProviderBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Formatters.Json;
using App.Metrics.Reporting.Builder;
using App.Metrics.Reporting.Http;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Builder for configuring metrics HTTP reporting using an
    ///     <see cref="IMetricsReportingBuilder" />.
    /// </summary>
    public static class MetricsHttpReporterProviderBuilder
    {
        /// <summary>
        ///     Add the <see cref="HttpMetricsReporterProvider" /> allowing metrics to be reported over HTTP.
        /// </summary>
        /// <param name="metricReporterProviderBuilder">
        ///     The <see cref="MetricsReporterProviderBuilder" /> used to configure metrics reporter providers.
        /// </param>
        /// <param name="setupAction">The HTTP reporting options to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsReportingBuilder" /> that can be used to further configure App Metrics Reporting.
        /// </returns>
        public static IMetricsReportingBuilder OverHttp(
            this MetricsReporterProviderBuilder metricReporterProviderBuilder,
            Action<MetricsReportingHttpOptions> setupAction = null)
        {
            if (metricReporterProviderBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricReporterProviderBuilder));
            }

            var options = new MetricsReportingHttpOptions();

            setupAction?.Invoke(options);

            if (options.MetricsOutputFormatter == null)
            {
                options.MetricsOutputFormatter = metricReporterProviderBuilder.Metrics.OutputMetricsFormatters.GetType<MetricsJsonOutputFormatter>();
            }

            var provider = new HttpMetricsReporterProvider(options);

            return metricReporterProviderBuilder.Using(provider);
        }
    }
}
