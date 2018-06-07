// <copyright file="MetricsReportingUdpOptions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Net.Http;
using App.Metrics.Filters;
using App.Metrics.Formatters;
using App.Metrics.Reporting.Udp.Client;

namespace App.Metrics.Reporting.Udp
{
    /// <summary>
    ///     Provides programmatic configuration of UDP Reporting in the App Metrics framework.
    /// </summary>
    public class MetricsReportingUdpOptions
    {
        public MetricsReportingUdpOptions()
        {
            UdpSettings = new UdpSettings();
            UdpPolicy = new UdpPolicy();
        }

        /// <summary>
        ///     Gets or sets the UDP policy settings which allows circuit breaker configuration to be adjusted
        /// </summary>
        /// <value>
        ///     The UDP policy.
        /// </value>
        public UdpPolicy UdpPolicy { get; set; }

        /// <summary>
        ///     Gets or sets the UDP client related settings.
        /// </summary>
        /// <value>
        ///     The UDP client settings.
        /// </value>
        public UdpSettings UdpSettings { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="IFilterMetrics" /> to use for just this reporter.
        /// </summary>
        /// <value>
        ///     The <see cref="IFilterMetrics" /> to use for this reporter.
        /// </value>
        public IFilterMetrics Filter { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="IMetricsOutputFormatter" /> used to write metrics.
        /// </summary>
        /// <value>
        ///     The <see cref="IMetricsOutputFormatter" /> used to write metrics.
        /// </value>
        public IMetricsOutputFormatter MetricsOutputFormatter { get; set; }

        /// <summary>
        ///     Gets or sets the interval between flushing metrics.
        /// </summary>
        public TimeSpan FlushInterval { get; set; }
    }
}