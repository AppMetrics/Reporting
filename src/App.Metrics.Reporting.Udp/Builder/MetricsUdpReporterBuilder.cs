// <copyright file="MetricsUdpReporterBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Builder;
using App.Metrics.Reporting.Udp;
using App.Metrics.Reporting.Udp.Client;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Builder for configuring metrics UDP reporting using an
    ///     <see cref="IMetricsReportingBuilder" />.
    /// </summary>
    public static class MetricsUdpReporterBuilder
    {
        /// <summary>
        ///     Add the <see cref="UdpMetricsReporter" /> allowing metrics to be reported over UDP.
        /// </summary>
        /// <param name="metricReporterProviderBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="options">The UDP reporting options to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder OverUdp(
            this IMetricsReportingBuilder metricReporterProviderBuilder,
            MetricsReportingUdpOptions options)
        {
            if (metricReporterProviderBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricReporterProviderBuilder));
            }

            UdpSettings.Validate(options.UdpSettings.Address, options.UdpSettings.Port);

            var provider = new UdpMetricsReporter(options);

            return metricReporterProviderBuilder.Using(provider);
        }

        /// <summary>
        ///     Add the <see cref="UdpMetricsReporter" /> allowing metrics to be reported over UDP.
        /// </summary>
        /// <param name="metricReporterProviderBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="setupAction">The UDP reporting options to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder OverUdp(
            this IMetricsReportingBuilder metricReporterProviderBuilder,
            Action<MetricsReportingUdpOptions> setupAction)
        {
            if (metricReporterProviderBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricReporterProviderBuilder));
            }

            var options = new MetricsReportingUdpOptions();

            setupAction?.Invoke(options);

            UdpSettings.Validate(options.UdpSettings.Address, options.UdpSettings.Port);

            var provider = new UdpMetricsReporter(options);

            return metricReporterProviderBuilder.Using(provider);
        }

        /// <summary>
        ///     Add the <see cref="UdpMetricsReporter" /> allowing metrics to be reported over UDP.
        /// </summary>
        /// <param name="metricReporterProviderBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="address">The UDP endpoint address where metrics are POSTed.</param>
        /// <param name="port">The UDP endpoint port where metrics are POSTed.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder OverUdp(
            this IMetricsReportingBuilder metricReporterProviderBuilder,
            string address,
            int port)
        {
            if (metricReporterProviderBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricReporterProviderBuilder));
            }

            UdpSettings.Validate(address, port);

            var options = new MetricsReportingUdpOptions
            {
                UdpSettings = new UdpSettings(address, port)
            };
            var provider = new UdpMetricsReporter(options);

            return metricReporterProviderBuilder.Using(provider);
        }

        /// <summary>
        ///     Add the <see cref="UdpMetricsReporter" /> allowing metrics to be reported over UDP.
        /// </summary>
        /// <param name="metricReporterProviderBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="address">The UDP endpoint address where metrics are POSTed.</param>
        /// <param name="port">The UDP endpoint port where metrics are POSTed.</param>
        /// <param name="flushInterval">
        ///     The <see cref="T:System.TimeSpan" /> interval used if intended to schedule metrics
        ///     reporting.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder OverUdp(
            this IMetricsReportingBuilder metricReporterProviderBuilder,
            string address,
            int port,
            TimeSpan flushInterval)
        {
            if (metricReporterProviderBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricReporterProviderBuilder));
            }

            UdpSettings.Validate(address, port);

            var options = new MetricsReportingUdpOptions
            {
                UdpSettings = new UdpSettings(address, port),
                FlushInterval = flushInterval
            };

            var provider = new UdpMetricsReporter(options);

            return metricReporterProviderBuilder.Using(provider);
        }
    }
}