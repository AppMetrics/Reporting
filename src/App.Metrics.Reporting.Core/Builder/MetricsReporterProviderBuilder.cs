// <copyright file="MetricsReporterProviderBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Formatters;
using App.Metrics.Reporting;
using App.Metrics.Reporting.Builder;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Builder for configuring <see cref="IMetricsReporterProvider"/>s used for reporting <see cref="IMetrics"/>s.
    /// </summary>
    public class MetricsReporterProviderBuilder
    {
        private readonly IMetricsReportingBuilder _metricsReportingBuilder;
        private readonly Action<IMetricsReporterProvider> _reporterProvider;

        internal MetricsReporterProviderBuilder(
            IMetricsReportingBuilder metricsReportingBuilder,
            Action<IMetricsReporterProvider> reporterProvider)
        {
            _metricsReportingBuilder = metricsReportingBuilder ?? throw new ArgumentNullException(nameof(metricsReportingBuilder));
            _reporterProvider = reporterProvider ?? throw new ArgumentNullException(nameof(reporterProvider));
        }

        public IMetricsRoot Metrics => _metricsReportingBuilder.Metrics;

        /// <summary>
        ///     <para>
        ///         Uses the specifed <see cref="IMetricsReporterProvider"/> as one of the available metrics reporter providers.
        ///     </para>
        ///     <para>
        ///         Multiple reporter providers can be used.
        ///     </para>
        /// </summary>
        /// <param name="provider">An <see cref="IMetricsReporterProvider"/> instance used report metrics.</param>
        /// <returns>
        ///     An <see cref="IMetricsReportingBuilder" /> that can be used to further configure App Metrics Reporting.
        /// </returns>
        public IMetricsReportingBuilder Using(IMetricsReporterProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            _reporterProvider(provider);

            return _metricsReportingBuilder;
        }

        /// <summary>
        ///     <para>
        ///         Uses the specifed <see cref="IMetricsReporterProvider"/> as one of the available metrics reporter providers.
        ///     </para>
        ///     <para>
        ///         Multiple reporter providers can be used.
        ///     </para>
        /// </summary>
        /// <typeparam name="TMetricsReporterProvider">An <see cref="IMetricsOutputFormatter"/> type used to format metric values when reporting.</typeparam>
        /// <returns>
        ///     An <see cref="IMetricsReportingBuilder" /> that can be used to further configure App Metrics Reporting.
        /// </returns>
        public IMetricsReportingBuilder Using<TMetricsReporterProvider>()
            where TMetricsReporterProvider : IMetricsReporterProvider, new()
        {
            _reporterProvider(new TMetricsReporterProvider());

            return _metricsReportingBuilder;
        }
    }
}
